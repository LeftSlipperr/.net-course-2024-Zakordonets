using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;
using Xunit;
using BankSystem.Infrastructure;
using ClientStorage;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    private static Infrastructure.ClientStorage _clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
    private ClientService _clientService = new ClientService(_clientStorage);
   
    [Fact]
    public void AddClientAddsClientSuccessfully()
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

        var clients = _clientService.Get(client);
        var result = clients.FirstOrDefault();
        var myClient = result.Key;
        
        Assert.Equal(myClient, client);
    }
    
    [Fact]
    public void UpdateClientPositiveTest()
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
        
        var updatedClient = new Client
        {
            Id = client.Id,
            PasNumber = client.PasNumber,
            Age = client.Age,
            Name = "updatedFullName",
            SecondName = "Bobson",
            ThirdName = "Bibson",
            PhoneNumber = client.PhoneNumber,
            AccountNumber = 123,
            Balance = 123
        };
        
        _clientService.UpdateClient(updatedClient);

        var newClient = _clientService.Get(client);
        
        Assert.Equal(newClient.Keys.FirstOrDefault(c => c.Id == updatedClient.Id), updatedClient);
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


    [Fact]
    public void AddAccountPositiveTest()
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
        
        var account = new Account
        {
            Amount = 1000,
            CurrencyName = "EUR"
        };
        
        _clientService.AddAccountToClient(client, account);
        
        var newClient = _clientService.Get(client);
        var accounts = newClient.Values;
        var newAccount = accounts.FirstOrDefault();
        
        Assert.Contains(newAccount, a => a.Id == account.Id);
    }

    [Fact]
    public void UpdateAccountPositiveTest()
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
    
        var oldAccount = new Account { Id = new Guid(), ClientId =client.Id, Amount = 1000, /*Currency = new Currency { CurrencyName = "USD", Symbol = "$" }*/ CurrencyName = "USD"};
        _clientService.AddAccountToClient(client, oldAccount);
    
        var newAccount = new Account { Id = oldAccount.Id, ClientId =client.Id, Amount = 2000, /*Currency = new Currency { CurrencyName = "USD", Symbol = "$" }*/ CurrencyName = "USD"};
        
        _clientService.UpdateAccount(newAccount);
        
        var newClient = _clientService.Get(client);
        var accounts = newClient.Values;
        var updatedAccount = accounts.FirstOrDefault();
        var myAccount = updatedAccount.First(a => a.Id.Equals(newAccount.Id));
        
        Assert.Equal(myAccount.Id, newAccount.Id);
    }

        
    [Fact]
    public void DeleteAccountPositiveTest()
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
    
        var account = new Account { Id = new Guid(), ClientId = client.Id, Amount = 1000, /*Currency = new Currency { CurrencyName = "EURO", Symbol = "E" }*/ CurrencyName = "EUR"};
        
        _clientService.AddAccountToClient(client, account);
        
        _clientService.DeleteAccount(account);
        
        var newClient = _clientService.Get(client);
        var accounts = newClient.Values;
        var updatedAccount = accounts.FirstOrDefault();
        Assert.DoesNotContain(updatedAccount, a => a.Id == account.Id);
    }
    
    [Fact]
    public void GetClientsByParametersWithPaginationTest()
    {
        var client1 = new Client
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

        var client2 = new Client
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            SecondName = "Doe",
            ThirdName = "Smith",
            Age = 25,
            PasNumber = "654321",
            PhoneNumber = "987654321",
            Balance = 1000
        };

        var client3 = new Client
        {
            Id = Guid.NewGuid(),
            Name = "Jack",
            SecondName = "Black",
            ThirdName = "White",
            Age = 28,
            PasNumber = "112233",
            PhoneNumber = "555555555",
            Balance = 1500
        };

        _clientStorage.Add(client1);
        _clientStorage.Add(client2);
        _clientStorage.Add(client3);
        
        var result = _clientStorage.GetClientsByParameters(name: "J", pageNumber: 1, pageSize: 2, sortBy: "Name");
        
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "Jack");
        Assert.Contains(result, c => c.Name == "Jane");
        
        var nextPageResult = _clientStorage.GetClientsByParameters(name: "J", pageNumber: 2, pageSize: 2, sortBy: "Name");
        
        Assert.Single(nextPageResult);
        Assert.Contains(nextPageResult, c => c.Name == "John");
    }




}