using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Models;
using ClientStorage;
using Microsoft.EntityFrameworkCore;
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
        
        var optionsBuilder = new DbContextOptionsBuilder<BankSystemDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Username=postgres;Password=mysecretpassword;Database=local");
        
        var dbContext = new BankSystemDbContext(optionsBuilder.Options);
        
        _clientStorage = new Storage.ClientStorage(dbContext);
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
            await _clientStorage.AddAsync(client.Key);

            foreach (var account in client.Value)
            {
                await _clientService.AddAccountAsync(client.Key, account);
            }
            
        }
        
        await Task.Run(async () =>
        {
            rateUpdaterService.UpdateRate(token);
        });
        
        await Task.Delay(10000);
        cancellationTokenSource.Cancel();
        
        var firstClient = clients.Keys.FirstOrDefault();
        var updatedAccounts = (await _clientService.GetAsync(firstClient.Id))[clients.Keys.FirstOrDefault()];
        foreach (var updatedAccount in updatedAccounts)
        {
            Assert.NotEqual(1000, updatedAccount.Amount);
        }
       
    }
}