using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Infrastructure;
using BankSystem.Models;
using static BankSystem.Infrastructure.ClientStorage;

namespace BankSystem.App.Tests;

public class ClientServiceTests
{
    private ClientService _clientService;
    private IClientStorage _clientStorage;

    public ClientServiceTests()
    {
        _clientStorage = new ClientStorage();
        _clientService = new ClientService(_clientStorage);
    }

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

            var allClients = _clientStorage.Get(c => true);
            Assert.Single(allClients);
            Assert.Equal(client, allClients.First().Key);
        }

        [Fact]
        public void AddClientThrowsUnderAgeClientExceptionIfClientIsUnder18()
        {
            Client client = new Client
            {
                FullName = "John Bobson",
                Age = 17,
                PasNumber = "987654321"
            };
            
            Assert.Throws<UnderAgeClientException>(() => _clientService.Add(client));
        }

        [Fact]
        public void AddClientThrowsMissingPassportExceptionIfNoPassport()
        {
            Client client = new Client
            {
                FullName = "John Bobson",
                Age = 30,
                PasNumber = ""
            };
            
            Assert.Throws<MissingPassportException>(() => _clientService.Add(client));
        }

        [Fact]
        public void AddAccountToClientAddsNewAccountSuccessfully()
        {
            Client client = new Client
            {
                FullName = "John Bobson",
                Age = 25,
                PasNumber = "123456789"
            };
            _clientService.Add(client);

            Account newAccount = new Account
            {
                Amount = 100,
                Currency = new Currency { CurrencyName = "EUR", Symbol = "€" }
            };
            
            _clientService.AddAccountToClient(client, newAccount);

            var storedClient = _clientStorage.Get(c => c == client).FirstOrDefault();
            Assert.Contains(storedClient.Value, a => a.Currency.CurrencyName == "EUR");
        }

        [Fact]
        public void EditAccountUpdatesAccountSuccessfully()
        {
            Client client = new Client
            {
                FullName = "John Bobson",
                Age = 25,
                PasNumber = "123456789"
            };
            _clientService.Add(client);
            
            Account oldAccount = new Account
            {
                Amount = 100,
                Currency = new Currency { CurrencyName = "RUB", Symbol = "R" }
            };
            _clientService.AddAccountToClient(client, oldAccount);

            Account newAccount = new Account
            {
                Amount = 500,
                Currency = new Currency { CurrencyName = "RUB", Symbol = "R" }
            };
            
            _clientStorage.UpdateAccount(client, newAccount);
            
            var storedClient = _clientStorage.Get(c => c.PasNumber == client.PasNumber).FirstOrDefault();
            Account updatedAccount = storedClient.Value.FirstOrDefault(a => a.Currency.CurrencyName == "RUB");
            Assert.Equal(100, updatedAccount.Amount);
        }
        
        [Fact]
        public void FilterClientsReturnsFilteredClients()
        {
            Client client1 = new Client
            {
                FullName = "Alice Johnson",
                Age = 30,
                PasNumber = "A12345678",
                PhoneNumber = "1234567890"
            };

            Client client2 = new Client
            {
                FullName = "Bob Smith",
                Age = 25,
                PasNumber = "B87654321",
                PhoneNumber = "0987654321"
            };

            Client client3 = new Client
            {
                FullName = "Charlie Brown",
                Age = 20,
                PasNumber = "C12398745",
                PhoneNumber = "555"
            };

            _clientService.Add(client1);
            _clientService.Add(client2);
            _clientService.Add(client3);
            
            Client filteredClients = _clientStorage.Get(c => c.FullName == client1.FullName).FirstOrDefault().Key;
            
            Assert.Equal(filteredClients.FullName, client1.FullName);
            
            filteredClients = _clientStorage.Get(c => c.Age == client2.Age).FirstOrDefault().Key;;
            
            Assert.Equal(filteredClients.Age, client2.Age);
            
            filteredClients = _clientStorage.Get(c => c.PasNumber == client3.PasNumber).FirstOrDefault().Key;
            
            Assert.Equal(filteredClients.PasNumber, client3.PasNumber);
        }
}