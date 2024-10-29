using System.Collections.Concurrent;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Infrastructure;
using BankSystem.Models;
using ClientStorage;
using Xunit;
using Xunit.Sdk;

namespace BankSystem.App.Tests;

public class ClientServiceTests
{
    private ClientService _clientService;
    private IClientStorage _clientStorage;
    private TestDataGenerator _testDataGenerator;

    public ClientServiceTests()
    {
        _clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
        _clientService = new ClientService(_clientStorage);
        _testDataGenerator = new TestDataGenerator();
    }

    [Fact]
    public async Task AddClientSuccessfully()
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "John",
            SecondName = "Doe",
            ThirdName = "Smith",
            Age = 30,
            PasNumber = "123456",
            PhoneNumber = "123456789",
            Balance = 500
        };
        
        await _clientService.AddAsync(client);
        
        var allClients = await _clientStorage.GetAsync(client.Id);
        Assert.Single(allClients);
        Assert.Equal(client, allClients.First().Key);
    }

    [Fact]
    public async Task AddClientThrowsUnderAgeClientExceptionIfClientIsUnder18()
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "John",
            SecondName = "Doe",
            ThirdName = "Smith",
            Age = 17,
            PasNumber = "123456",
            PhoneNumber = "123456789",
            Balance = 500
        };
        
        await Assert.ThrowsAsync<UnderAgeClientException>( async () => await _clientService.AddAsync(client));
    }

    [Fact]
    public async Task AddAccountToClientAddsNewAccountSuccessfully()
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "John",
            SecondName = "Doe",
            ThirdName = "Smith",
            Age = 30,
            PasNumber = "123456",
            PhoneNumber = "123456789",
            Balance = 500
        };
        await _clientService.AddAsync(client);

        Account newAccount = new Account
        {
            Id = new Guid(),
            ClientId = client.Id,
            Amount = 100,
            CurrencyName = "USD"
        };
        
        await _clientService.AddAccountToClientAsync(client, newAccount);

        var storedClient = await _clientStorage.GetAsync(client.Id);
        Assert.Contains(storedClient.FirstOrDefault().Value, a => a.CurrencyName == "USD");
    }

    [Fact]
    public async Task EditAccountUpdatesAccountSuccessfully()
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "John",
            SecondName = "Doe",
            ThirdName = "Smith",
            Age = 30,
            PasNumber = "123456",
            PhoneNumber = "123456789",
            Balance = 500
        };
        await _clientService.AddAsync(client);
        
        Account oldAccount = new Account
        {
            Id = new Guid(),
            ClientId = client.Id,
            Amount = 100,
            CurrencyName = "RUB"
        };
        await _clientService.AddAccountToClientAsync(client, oldAccount);

        Account updatedAccount = new Account
        {
            Id = oldAccount.Id,
            ClientId = client.Id,
            Amount = 500,
            CurrencyName = "RUB"
        };
        
        await _clientService.UpdateAccountAsync(updatedAccount);
        
        var storedClient = await _clientStorage.GetAsync(client.Id);
        Account resultAccount = storedClient.FirstOrDefault().Value.FirstOrDefault(a => a.Id == oldAccount.Id);
        Assert.Equal(500, resultAccount.Amount);
    }
    
    [Fact]
    public async Task Get()
    {

        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "John",
            SecondName = "Doe",
            ThirdName = "Smith",
            Age = 30,
            PasNumber = "123456",
            PhoneNumber = "123456789",
            Balance = 500
        };
        
        await _clientService.AddAsync(client);
        
        var findById = await _clientStorage.GetAsync(client.Id);
        
        Assert.NotEmpty(findById);
        Assert.Equal(client.Name, findById.First().Key.Name);
    }
    
    [Fact]
    public async Task DeleteClientPositiveTest()
    {
        Client client = new Client
        {
            Id = new Guid(),
            Name = "John",
            SecondName = "Bobson",
            ThirdName = "Bibson",
            Age = 25,
            PasNumber = "123456789",
            PhoneNumber = "1234567",
            AccountNumber = 123,
            Balance = 123
        };
        
        await _clientService.AddAsync(client);
        
        await _clientService.DeleteClientAsync(client);
        
        var newClient = await _clientService.GetAsync(client);

        Assert.NotEqual(newClient.Keys.FirstOrDefault(c => c.Id == client.Id), client);
    }

    [Fact]
    public async Task WithdrawFromAccountPositiveTest()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var clients = new ConcurrentDictionary<Client, List<Account>>();
        
        _testDataGenerator.ClientsList(10);
        var clientsWithAccounts = _testDataGenerator.ClientsDictionary();

        await Task.Run(async () =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                _testDataGenerator.ClientsList(10);
                var clientsWithAccounts = _testDataGenerator.ClientsDictionary();
            
                foreach (var client in clientsWithAccounts)
                {
                    var clientAccounts = await _clientService.GetAsync(client.Key);
                    if (clientAccounts.Count == 0)
                    {
                        await _clientService.AddAsync(client.Key);

                        foreach (var account in client.Value)
                        {
                            await _clientService.AddAccountToClientAsync(client.Key, account);
                        }

                        clients.TryAdd(client.Key, client.Value);
                    }
                }
            }
        }, cts.Token);

        var tasks = new List<Task>();
        decimal amountToWithdraw = 100;
        
        foreach (var client in clients.Keys)
        {
            tasks.Add(Task.Run(() => _clientService.WithdrawFromAccountAsync(client, amountToWithdraw)));
        }

        await Task.WhenAll(tasks);

        foreach (var client in clients)
        {
            var clientAccounts = await _clientService.GetAsync(client.Key);
            var updatedAccount = clientAccounts.Values.FirstOrDefault();
            Assert.Equal(0, updatedAccount.FirstOrDefault().Amount);
        }
    }


}
