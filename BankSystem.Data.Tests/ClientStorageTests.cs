using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;
using Xunit;
using BankSystem.Infrastructure;
using ClientStorage;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    
    private readonly Storage.ClientStorage _clientStorage;
    private readonly ClientService _clientService;

    public ClientStorageTests()
    {
        var options = new DbContextOptionsBuilder<BankSystemDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _clientStorage = new Storage.ClientStorage(new BankSystemDbContext(options));
    }
   
    [Fact]
    public async Task AddClientAddsClientSuccessfully()
    {
        Client client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "John",
            SecondName = "Bobson",
            ThirdName = "Bibson",
            Age = 25,
            PasNumber = "123456789",
            PhoneNumber = "1234567",
            AccountNumber = 123,
            Balance = 123
        };
        
        await _clientStorage.AddAsync(client);

        var clients = await _clientStorage.GetAsync(client.Id);
        var result = clients.FirstOrDefault();
        var myClient = result.Key;
        
        Assert.Equal(myClient, client);
    }
    
    [Fact]
    public async Task UpdateClientPositiveTest()
    {
        
        Client client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "John",
            SecondName = "Bobson",
            ThirdName = "Bibson",
            Age = 25,
            PasNumber = "123456789",
            PhoneNumber = "1234567",
            AccountNumber = 123,
            Balance = 123
        };
        
        await _clientStorage.AddAsync(client);
        
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
        
        await _clientStorage.UpdateAsync(updatedClient.Id, updatedClient);

        var newClient =  await _clientStorage.GetAsync(client.Id);
        
        Assert.Equal(newClient.Keys.FirstOrDefault(c => c.Id == updatedClient.Id), updatedClient);
    }
    
    [Fact]
    public async Task DeleteClientPositiveTest()
    {
        Client client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "John",
            SecondName = "Bobson",
            ThirdName = "Bibson",
            Age = 25,
            PasNumber = "123456789",
            PhoneNumber = "1234567",
            AccountNumber = 123,
            Balance = 123
        };
        
        await _clientStorage.AddAsync(client);
        
        await _clientStorage.DeleteAsync(client.Id);
        
        var newClient = await _clientStorage.GetAsync(client.Id);

        Assert.NotEqual(newClient.Keys.FirstOrDefault(c => c.Id == client.Id), client);
    }


    [Fact]
    public async Task AddAccountPositiveTest()
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
        
        await _clientStorage.AddAsync(client);
        
        var account = new Account
        {
            Amount = 1000,
            CurrencyName = "EUR"
        };
        
        await _clientStorage.AddAccountAsync(client, account);
        
        var newClient = await _clientStorage.GetAsync(client.Id);
        var accounts = newClient.Values;
        var newAccount = accounts.FirstOrDefault();
        
        Assert.Contains(newAccount, a => a.Id == account.Id);
    }

    [Fact]
    public async Task UpdateAccountPositiveTest()
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
    
        await _clientStorage.AddAsync(client);
    
        var oldAccount = new Account { Id = new Guid(), ClientId =client.Id, Amount = 1000, /*Currency = new Currency { CurrencyName = "USD", Symbol = "$" }*/ CurrencyName = "USD"};
        await _clientStorage.AddAccountAsync(client, oldAccount);
    
        var newAccount = new Account { Id = oldAccount.Id, ClientId =client.Id, Amount = 2000, /*Currency = new Currency { CurrencyName = "USD", Symbol = "$" }*/ CurrencyName = "USD"};
        
        await _clientStorage.UpdateAccountAsync(newAccount);
        
        var newClient = await _clientStorage.GetAsync(client.Id);
        var accounts = newClient.Values;
        var updatedAccount = accounts.FirstOrDefault();
        var myAccount = updatedAccount.First(a => a.Id.Equals(newAccount.Id));
        
        Assert.Equal(myAccount.Id, newAccount.Id);
    }

        
    [Fact]
    public async Task DeleteAccountPositiveTest()
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
    
        await _clientStorage.AddAsync(client);
    
        var account = new Account { Id = new Guid(), ClientId = client.Id, Amount = 1000, /*Currency = new Currency { CurrencyName = "EURO", Symbol = "E" }*/ CurrencyName = "EUR"};
        
        await _clientStorage.AddAccountAsync(client, account);
        
        await _clientStorage.DeleteAccountAsync(account.Id);
        
        var newClient = await _clientStorage.GetAsync(client.Id);
        var accounts = newClient.Values;
        var updatedAccount = accounts.FirstOrDefault();
        Assert.DoesNotContain(updatedAccount, a => a.Id == account.Id);
    }
    
    [Fact]
    public async Task GetClientsByParametersWithPaginationTest()
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

        await _clientStorage.AddAsync(client1);
        await _clientStorage.AddAsync(client2);
        await _clientStorage.AddAsync(client3);
        
        var result = await _clientStorage.GetClientsByParametersAsync(name: "J", pageNumber: 1, pageSize: 2, sortBy: "Name");
        
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "Jack");
        Assert.Contains(result, c => c.Name == "Jane");
        
        var nextPageResult = await _clientStorage.GetClientsByParametersAsync(name: "J", pageNumber: 2, pageSize: 2, sortBy: "Name");
        
        Assert.Single(nextPageResult);
        Assert.Contains(nextPageResult, c => c.Name == "John");
    }




}