using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;
using Xunit;
using BankSystem.Infrastructure;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    private static ClientStorage _clientStorage = new ClientStorage();
    private static TestDataGenerator _testDataGenerator = new TestDataGenerator();
    private List<Client> clients = _testDataGenerator.ClientsList();
    private Dictionary<Client, List<Account>> _clientsAccount = _testDataGenerator.ClientsDictionary();
    private ClientService _clientService = new ClientService(_clientStorage);
   
    [Fact]
    public void AddClientAddsClientSuccessfully()
    {
        Client client = new Client
        {
            FullName = "John Bobson",
            Age = 25,
            PasNumber = "123456789"
        };
        
        _clientService.Add(client);
        
        bool result = _clientService.IsClientAdded(client);
        
        Assert.True(result, "Клиент не был успешно добавлен");
    }
    
    [Fact]
    public void UpdateClientPositiveTest()
    {
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        
        Client client = new Client
        {
            FullName = "John Bobson",
            Age = 25,
            PasNumber = "123"
        };
        
        _clientService.Add(client);
        
        var updatedClient = new Client
        {
            PasNumber = client.PasNumber,
            Age = client.Age,
            FullName = "updatedFullName",
            PhoneNumber = client.PhoneNumber
        };
        
        _clientService.UpdateClient(updatedClient);
        
        bool isUpdated = _clientService.IsClientUpdated(updatedClient.PasNumber, updatedClient.FullName);
        
        Assert.True(isUpdated, "Клиент не был успешно обновлен");
    }
    
    [Fact]
    public void DeleteClientPositiveTest()
    {
        Client client = new Client
        {
            FullName = "John Bobson",
            Age = 25,
            PasNumber = "123"
        };
        
        _clientService.Add(client);
        
        _clientService.DeleteClient(client);
        
        bool isDeleted = _clientService.IsClientDeleted(client.PasNumber);
    
        Assert.True(isDeleted);
    }


    [Fact]
    public void AddAccountPositiveTest()
    {
        Client client = new Client
        {
            FullName = "John Bobson",
            Age = 25,
            PasNumber = "123"
        };
        
        _clientService.Add(client);
        
        var account = new Account { Amount = 1000, Currency = new Currency { CurrencyName = "EURO", Symbol = "E" }};
        
        _clientService.AddAccountToClient(client, account);
        
        bool accountExists = _clientService.DoesClientHaveAccount(client, account);
        Assert.True(accountExists);
    }

    [Fact]
    public void UpdateAccountPositiveTest()
    {
        Client client = new Client
        {
            FullName = "John Bobson",
            Age = 25,
            PasNumber = "123"
        };
    
        _clientService.Add(client);
    
        var oldAccount = new Account { Amount = 1000, Currency = new Currency { CurrencyName = "USD", Symbol = "$" }};
        _clientService.AddAccountToClient(client, oldAccount);
    
        var newAccount = new Account { Amount = 2000, Currency = new Currency { CurrencyName = "USD", Symbol = "$" }};
        
        _clientService.UpdateAccount(client, newAccount);
        
        bool accountUpdated = _clientService.DoesClientHaveAccount(client, newAccount);
        Assert.True(accountUpdated);
    }

        
    [Fact]
    public void DeleteAccountPositiveTest()
    {
        Client client = new Client
        {
            FullName = "John Bobson",
            Age = 25,
            PasNumber = "123"
        };
    
        _clientService.Add(client);
    
        var account = new Account { Amount = 1000, Currency = new Currency { CurrencyName = "EURO", Symbol = "E" }};
        
        _clientService.AddAccountToClient(client, account);
        
        _clientService.DeleteAccount(client, account);
        
        bool accountDeleted = _clientService.DoesClientHaveAccount(client, account);
        Assert.False(accountDeleted);
    }
    
    [Fact]
    public void GetClientsByFilter_ReturnsFilteredClients()
    {
        var clientStorage = new ClientStorage();
    
        var client1 = new Client { FullName = "John Bobson", PasNumber = "123", Age = 30 };
        var client2 = new Client { FullName = "Jane Smith", PasNumber = "456", Age = 25 };
        var client3 = new Client { FullName = "Alice Johnson", PasNumber = "789", Age = 40 };
        
        clientStorage.Add(client1);
        clientStorage.Add(client2);
        clientStorage.Add(client3);

        var account1 = new Account { Amount = 1000, Currency = new Currency { CurrencyName = "USD", Symbol = "$" } };
        var account2 = new Account { Amount = 2000, Currency = new Currency { CurrencyName = "EUR", Symbol = "€" } };
        
        clientStorage.AddAccount(client1, account1);
        clientStorage.AddAccount(client2, account2);

        var result = clientStorage.Get(c => c.Age > 30); 
        
        Assert.Single(result);
        Assert.Contains(client3, result.Keys);
    }



}