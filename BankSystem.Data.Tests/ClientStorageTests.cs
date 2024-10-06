using BankSystem.App.Services;
using BankSystem.Models;
using Xunit;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    [Fact]
    public void AddClientAddsClientSuccessfuly()
    {
        ClientStorage.ClientStorage clientStorage = new ClientStorage.ClientStorage();
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        List<Client> clients = testDataGenerator.ClientsList();
        Dictionary<Client, List<Account>> _clientsAccount= testDataGenerator.ClientsDictionary();
        
        foreach (var client in _clientsAccount)
        {
            clientStorage.AddClient(client.Key, client.Value);
        }
        
        Assert.Equal(1000, clientStorage.GetAllClients().Count());
    }
    
    [Fact]
    public void GetYoungestClientClientReturnsCorrectClient()
    {
        ClientStorage.ClientStorage clientStorage = new ClientStorage.ClientStorage();
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        List<Client> clients = testDataGenerator.ClientsList();
        Dictionary<Client, List<Account>> _clientsAccount= testDataGenerator.ClientsDictionary();
        
        foreach (var client in _clientsAccount)
        {
            clientStorage.AddClient(client.Key, client.Value);
        }
        Client yongestClient = _clientsAccount.OrderBy(c => c.Key.Age).FirstOrDefault().Key;
        Client youngest = clientStorage.GetYoungestClient();
        
        Assert.Equal(youngest, yongestClient);
    }

    [Fact]
    public void GetOldestClientReturnsCorrectClient()
    {
        ClientStorage.ClientStorage clientStorage = new ClientStorage.ClientStorage();
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        List<Client> clients = testDataGenerator.ClientsList();
        Dictionary<Client, List<Account>> _clientsAccount= testDataGenerator.ClientsDictionary();
        
        foreach (var client in _clientsAccount)
        {
            clientStorage.AddClient(client.Key, client.Value);
        }
        Client oldestClient = _clientsAccount.OrderByDescending(c => c.Key.Age).FirstOrDefault().Key;
        Client oldest = clientStorage.GetOldestClient();
        
        Assert.Equal(oldest, oldestClient);
    }

    [Fact]
    public void GetAverageAgeReturnsCorrectValue()
    {
        ClientStorage.ClientStorage clientStorage = new ClientStorage.ClientStorage();
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        List<Client> clients = testDataGenerator.ClientsList();
        Dictionary<Client, List<Account>> _clientsAccount= testDataGenerator.ClientsDictionary();
        
        foreach (var client in _clientsAccount)
        {
            clientStorage.AddClient(client.Key, client.Value);
        }
        double avAgeClient = _clientsAccount.Average(c => c.Key.Age);
        double avgAge = clientStorage.GetAverageAge();
        
        Assert.Equal(avgAge, avAgeClient);
    }
}