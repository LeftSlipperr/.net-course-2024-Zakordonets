using System.Collections.Concurrent;
using BankSystem.App.Services;
using BankSystem.Models;
using Xunit;
using Xunit.Abstractions;

namespace ExportTool;

public class ThreadAndTaskTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly TestDataGenerator _testDataGenerator = new TestDataGenerator();
    private readonly ExportService _exportService = new ExportService();
    private List<Client> _clients;
    private ConcurrentDictionary<string, Client> _clientsBag = new ConcurrentDictionary<string, Client>();
    private readonly object lock1 = new object();
    private readonly string _directoryPath = Path.Combine("C:", "Users", "Admin", "Desktop", "ClientFiles");

    public ThreadAndTaskTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void ExportToJsonClients()
    {
        Directory.CreateDirectory(_directoryPath);
        
        Thread threadCreateClients = new Thread(() =>
        {
                _clients = _testDataGenerator.ClientsList(1000);
                for (int i = 0; i < _clients.Count; i++)
                {
                    _clientsBag.TryAdd(_clients[i].PasNumber, _clients[i]);
                }
        });
        Thread threadCreateClients2 = new Thread(() =>
        {
                _clients = _testDataGenerator.ClientsList(1000);
                for (int i = 0 ; i < _clients.Count ; i++)
                {
                    _clientsBag.TryAdd(_clients[i].PasNumber, _clients[i]);
                }
        });
        Thread threadCreateClients3 = new Thread(() =>
        {
                _clients = _testDataGenerator.ClientsList(1000);
                for (int i = 0 ; i < _clients.Count ; i++)
                {
                    _clientsBag.TryAdd(_clients[i].PasNumber, _clients[i]);
                }
        });
        
        Thread threadSerialization = new Thread(() =>
        {
            foreach (var client in _clientsBag)
            {
                _exportService.ItemsSerialization(client.Value, _directoryPath);
            }
        });
        threadCreateClients .Start();
        threadCreateClients2.Start();
        threadCreateClients3.Start(); 
        
        threadCreateClients3.Join();
        threadCreateClients2.Join();
        threadCreateClients.Join();
        
        _testOutputHelper.WriteLine(_clientsBag.Count.ToString());

         threadSerialization.Start();
         threadSerialization.Join();
        
        var files = Directory.GetFiles(_directoryPath );
        Assert.NotEmpty(files);
    }

    [Fact]
    public void ImportFromJsonClients()
    {
        Thread threadDeserealization = new Thread(() =>
        {
                _clients = _exportService.ItemsDeserialization<Client>(_directoryPath);
        });
        
        threadDeserealization.Start();
        threadDeserealization.Join();
        
        Assert.NotEmpty(_clients);
    }

    [Fact]
    public void AccountUpdateBalance()
    {
        
        var account = _testDataGenerator.CreateAccount();

        Thread threadAccount1 = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                lock (lock1)
                {
                    account.Amount = account.Amount + 100;
                }
            }
            
        });
        
        Thread threadAccount2 = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                lock (lock1)
                {
                    account.Amount = account.Amount + 100;
                }
            }
            
        });
        
        threadAccount1.Start();
        threadAccount2.Start();
        
        threadAccount1.Join();
        threadAccount2.Join();
        
        Assert.Equal(2000, account.Amount);
    }
}
