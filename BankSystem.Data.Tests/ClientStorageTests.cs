using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Models;
using Xunit;
using BankSystem.Infrastructure;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    private  ClientStorage _clientStorage = new ClientStorage();
    private static TestDataGenerator _testDataGenerator = new TestDataGenerator();
    private List<Client> clients = _testDataGenerator.ClientsList();
    private Dictionary<Client, List<Account>> _clientsAccount = _testDataGenerator.ClientsDictionary();
    [Fact]
    public void AddClientAddsClientSuccessfuly()
    {
        foreach (var client in _clientsAccount)
        {
            _clientStorage.Add(client.Key);
        }
        
        Assert.Equal(1000, _clientStorage.GetAllClients().Count);
    }
    
    [Fact]
    public void UpdateClientPositiveTest()
    {
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        Client client = new Client();
        client.FullName = "John Bobson";
        client.PasNumber = "123";
        
        _clientStorage.Add(client);
        
        var updatedClient = new Client();
        updatedClient.PasNumber = client.PasNumber;
        updatedClient.FullName = "updatedFullName";
        updatedClient.PhoneNumber = client.PhoneNumber;
        
        _clientStorage.Update(updatedClient);
        
        var clients = _clientStorage.GetAllClients();
        Assert.Contains(updatedClient, clients);
    }
    
    
    [Fact]
    public void DeleteClientPositiveTest()
    {
        Client client = new Client();
        client.FullName = "John Bobson";
        client.PasNumber = "123";
        
        _clientStorage.Add(client);
        
        _clientStorage.Delete(client);
        
        var clients = _clientStorage.GetAllClients();
        Assert.DoesNotContain(client, clients);
    }

    [Fact]
    public void AddAccountPositiveTest()
    {
        Client client = new Client();
        client.FullName = "John Bobson";
        client.PasNumber = "123";
        
        
        _clientStorage.Add(client);
        var account = new Account(){Amount = 1000 , Currency = new Currency(){CurrencyName = "EURO", Symbol = "E"}};
        
        _clientStorage.AddAccount(client, account);
        
        var accounts = _clientStorage.GetClientAccounts(client);
        Assert.Contains(account, accounts);
    }
    
    [Fact]
    public void UpdateAccountPositiveTest()
    {
        Client client = new Client();
        client.FullName = "John Bobson";
        client.PasNumber = "123";
        
        _clientStorage.Add(client);
        
        var oldAccount = new Account(){Amount = 1000 , Currency = new Currency(){CurrencyName = "USD", Symbol = "$"}};
        _clientStorage.AddAccount(client, oldAccount);
        var newaAccount = new Account(){Amount = 2000 , Currency = new Currency(){CurrencyName = "USD", Symbol = "$"}};
        
        _clientStorage.UpdateAccount(client, newaAccount);
        
        var accounts = _clientStorage.GetClientAccounts(client);
        Assert.Contains(newaAccount, accounts);
    }
        
    [Fact]
    public void DeleteAccount_PositiveTest()
    {
        Client client = new Client();
        client.FullName = "John Bobson";
        client.PasNumber = "123";
        
        _clientStorage.Add(client);
    
        var account1 = new Account(){Amount = 1000 , Currency = new Currency(){CurrencyName = "EURO", Symbol = "E"}};

        _clientStorage.AddAccount(client, account1);
        _clientStorage.DeleteAccount(client, account1);
        
        var accounts = _clientStorage.GetClientAccounts(client);
            
        Assert.Single(accounts); ;
    }

}