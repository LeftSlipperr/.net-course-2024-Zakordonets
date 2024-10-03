using BankSystem.App.Services;
using BankSystem.Models;
using Xunit;

namespace BankSystem.Data.Tests;

public class ClientStorageTests
{
    ClientStorage.ClientStorage clientStorage = new ClientStorage.ClientStorage();
    static TestDataGenerator testDataGenerator = new TestDataGenerator();
    List<Client> clients = testDataGenerator.ClientsList();

    [Fact]
    public void AddClientAddsClientSuccessfuly()
    {
        foreach (var client in clients)
        {
            clientStorage.AddClient(client);
        }
        
        Assert.Equal(1000, clientStorage.GetAllClients().Count());
    }
    
    [Fact]
    public void GetYoungestClientClientReturnsCorrectClient()
    {
        foreach (var client in clients)
        {
            clientStorage.AddClient(client);
        }
        Client yongestClient = clients.OrderBy(c => c.Age).FirstOrDefault();
        Client youngest = clientStorage.GetYoungestClient();
        
        Assert.Equal(youngest.FullName, yongestClient.FullName);
    }

    [Fact]
    public void GetOldestClientReturnsCorrectClient()
    {
        foreach (var client in clients)
        {
            clientStorage.AddClient(client);
        }
        Client oldestClient = clients.OrderByDescending(c => c.Age).FirstOrDefault();
        Client oldest = clientStorage.GetOldestClient();
        
        Assert.Equal(oldest.FullName, oldestClient.FullName);
    }

    [Fact]
    public void GetAverageAgeReturnsCorrectValue()
    {
        foreach (var client in clients)
        {
            clientStorage.AddClient(client);
        }
        double avAgeClient = clients.Average(c => c.Age);
        double avgAge = clientStorage.GetAverageAge();
        
        Assert.Equal(avgAge, avAgeClient);
    }
}