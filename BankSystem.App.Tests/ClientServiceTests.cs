using System.Collections.Concurrent;
using AutoMapper;
using BankSystem.App.DTO;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;
using ClientStorage;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.App.Tests;

public class ClientServiceTests
{
    private readonly IClientStorage _clientStorage;
    private readonly ClientService _clientService;
    private readonly TestDataGenerator _dataGenerator;
    private readonly IMapper _mapper;

    public ClientServiceTests()
    {
        var options = new DbContextOptionsBuilder<BankSystemDbContext>()
            .UseNpgsql("Host=localhost;Port=5434;Username=postgres;Password=mysecretpassword;Database=local")
            .Options;
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = mapperConfig.CreateMapper();
        _clientStorage = new Data.Storage.ClientStorage(new BankSystemDbContext(options));
        _clientService = new ClientService(_clientStorage, _mapper);
        _dataGenerator = new TestDataGenerator();
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
        
        var clientDto = _mapper.Map<ClientDto>(client);
        
        await _clientService.AddClientAsync(clientDto);
        
        var allClients = await _clientStorage.GetClientsByParametersAsync("John");
        Assert.Single(allClients);
        Assert.Equal(client, allClients.First());
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
        var clientDto = _mapper.Map<ClientDto>(client);
        
        await Assert.ThrowsAsync<UnderAgeClientException>( async () => await _clientService.AddClientAsync(clientDto));
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
        var clientDto = _mapper.Map<ClientDto>(client);
        
        await _clientService.AddClientAsync(clientDto);

        Account newAccount = new Account
        {
            Id = new Guid(),
            ClientId = client.Id,
            Amount = 100,
            CurrencyName = "USD"
        };
        
        var findClient = await _clientStorage.GetClientsByParametersAsync("John");
        
        await _clientService.AddAccountAsync(findClient.FirstOrDefault(), newAccount);

        var storedClient = await _clientStorage.GetAllClientsWithAccountsAsync();
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
        var clientDto = _mapper.Map<ClientDto>(client);
        
        await _clientService.AddClientAsync(clientDto);
        
        Account oldAccount = new Account
        {
            Id = new Guid(),
            ClientId = client.Id,
            Amount = 100,
            CurrencyName = "RUB"
        };
        
        var findClient = await _clientStorage.GetClientsByParametersAsync("John");
        
        await _clientService.AddAccountAsync(findClient.FirstOrDefault(), oldAccount);

        Account updatedAccount = new Account
        {
            Id = oldAccount.Id,
            ClientId = client.Id,
            Amount = 500,
            CurrencyName = "RUB"
        };
        
        await _clientService.UpdateAccountAsync(updatedAccount);
        
        var storedClient = await _clientStorage.GetAsync(findClient.FirstOrDefault().Id);
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
        var clientDto = _mapper.Map<ClientDto>(client);
        
        await _clientService.AddClientAsync(clientDto);
        
        var findClient = await _clientStorage.GetClientsByParametersAsync("John");
        
        var findById = await _clientStorage.GetAsync(findClient.FirstOrDefault().Id);
        
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
        var clientDto = _mapper.Map<ClientDto>(client);
        
        await _clientService.AddClientAsync(clientDto);
        
        await _clientService.DeleteClientAsync(client.Id);
        
        var newClient = await _clientService.GetAsync(client.Id);

        Assert.NotEqual(newClient.Keys.FirstOrDefault(c => c.Id == client.Id), client);
    }

    [Fact]
    public async Task WithdrawFromAccountPositiveTest()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var clients = new ConcurrentDictionary<Client, List<Account>>();

        await Task.Run(async () =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                _dataGenerator.ClientsList(10);
                var clientsWithAccounts = _dataGenerator.ClientsDictionary();
            
                foreach (var client in clientsWithAccounts)
                {
                    var clientAccounts = await _clientService.GetAsync(client.Key.Id);
                    if (clientAccounts.Count == 0)
                    {
                        var clientDto = _mapper.Map<ClientDto>(client.Key);
                        var id = await _clientService.AddClientAsync(clientDto);
                        var findClient = await _clientStorage.GetAsync(id);
                        

                        foreach (var account in client.Value)
                        {
                            
                            await _clientService.AddAccountAsync(findClient.Keys.FirstOrDefault(), account);
                        }

                        clients.TryAdd(findClient.Keys.FirstOrDefault(), client.Value);
                    }
                }
            }
        }, cts.Token);
        
        var tasks = new List<Task>();
        decimal amountToWithdraw = 100;
        
        foreach (var client in clients.Keys)
        {
            tasks.Add(Task.Run( async () => await _clientService.WithdrawFromAccountAsync(client, amountToWithdraw)));
        }

        await Task.WhenAll(tasks);

        foreach (var client in clients)
        {
            
            var clientAccounts = await _clientService.GetAsync(client.Key.Id);
            var updatedAccount = clientAccounts.Values.FirstOrDefault();
            Assert.Equal(0, updatedAccount.FirstOrDefault().Amount);
        }
    }
}
