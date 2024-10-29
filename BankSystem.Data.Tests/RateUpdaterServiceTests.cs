using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Models;
using ClientStorage;
using Xunit;

namespace BankSystem.Data.Tests;

public class RateUpdaterServiceTests
{
    private RateUpdaterService rateUpdaterService;
    private IClientStorage _clientStorage;
    private TestDataGenerator _testDataGenerator;
    private ClientService _clientService;

    public RateUpdaterServiceTests()
    {
        _testDataGenerator = new TestDataGenerator();
        _clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
        _clientService = new ClientService(_clientStorage);
        rateUpdaterService = new RateUpdaterService(_clientStorage);
    }
    
    [Fact]
    public async Task RateUpdaterServiceSuccessfully()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        
        _testDataGenerator.ClientsList(10);
        var clients = _testDataGenerator.ClientsDictionary();
        
        
        foreach (var client in clients)
        {
            await _clientService.AddAsync(client.Key);

            foreach (var account in client.Value)
            {
                await _clientService.AddAccountToClientAsync(client.Key, account);
            }
            
        }
        
        await Task.Run(async () =>
        {
            rateUpdaterService.UpdateRate(token);
        });
        
        await Task.Delay(10000);
        cancellationTokenSource.Cancel();
        
        var firstClient = clients.Keys.FirstOrDefault();
        var updatedAccounts = (await _clientService.GetAsync(firstClient))[clients.Keys.FirstOrDefault()];
        foreach (var updatedAccount in updatedAccounts)
        {
            Assert.NotEqual(1000, updatedAccount.Amount);
        }
       
    }
}