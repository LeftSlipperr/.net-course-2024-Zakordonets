using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Infrastructure;
using BankSystem.Models;
using ClientStorage;
using Xunit;

namespace BankSystem.App.Tests;

public class ClientServiceTests
{
    private ClientService _clientService;
    private IClientStorage _clientStorage;

    public ClientServiceTests()
    {
        _clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
        _clientService = new ClientService(_clientStorage);
    }

    [Fact]
    public void AddClientSuccessfully()
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
        
        _clientService.Add(client);
        
        var allClients = _clientStorage.Get(client.Id);
        Assert.Single(allClients);
        Assert.Equal(client, allClients.First().Key);
    }

    [Fact]
    public void AddClientThrowsUnderAgeClientExceptionIfClientIsUnder18()
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
        
        Assert.Throws<UnderAgeClientException>(() => _clientService.Add(client));
    }

    [Fact]
    public void AddAccountToClientAddsNewAccountSuccessfully()
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
        _clientService.Add(client);

        Account newAccount = new Account
        {
            Id = new Guid(),
            ClientId = client.Id,
            Amount = 100,
            CurrencyName = "USD"
        };
        
        _clientService.AddAccountToClient(client, newAccount);
        
        var storedClient = _clientStorage.Get(client.Id).FirstOrDefault();
        Assert.Contains(storedClient.Value, a => a.CurrencyName == "USD");
    }

    [Fact]
    public void EditAccountUpdatesAccountSuccessfully()
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
        _clientService.Add(client);
        
        Account oldAccount = new Account
        {
            Id = new Guid(),
            ClientId = client.Id,
            Amount = 100,
            CurrencyName = "RUB"
        };
        _clientService.AddAccountToClient(client, oldAccount);

        Account updatedAccount = new Account
        {
            Id = oldAccount.Id,
            ClientId = client.Id,
            Amount = 500,
            CurrencyName = "RUB"
        };
        
        _clientService.UpdateAccount(updatedAccount);
        
        var storedClient = _clientStorage.Get(client.Id).FirstOrDefault();
        Account resultAccount = storedClient.Value.FirstOrDefault(a => a.Id == oldAccount.Id);
        Assert.Equal(500, resultAccount.Amount);
    }
    
    [Fact]
    public void Get()
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
        
        _clientService.Add(client);
        
        var findById = _clientStorage.Get(client.Id);
        
        Assert.NotEmpty(findById);
        Assert.Equal(client.Name, findById.First().Key.Name);
    }
    
    [Fact]
    public void DeleteClientPositiveTest()
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
        
        _clientService.Add(client);
        
        _clientService.DeleteClient(client);
        
        var newClient = _clientService.Get(client);

        Assert.NotEqual(newClient.Keys.FirstOrDefault(c => c.Id == client.Id), client);
    }
}
