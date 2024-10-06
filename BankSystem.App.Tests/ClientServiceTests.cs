using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Tests;

public class ClientServiceTests
{
        private ClientService _clientService;
        private ClientStorage.ClientStorage _clientStorage;

        public ClientServiceTests()
        {
            _clientStorage = new ClientStorage.ClientStorage();
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
            
            _clientService.AddClient(client);
            
            Dictionary<Client, List<Account>> storedClients = _clientStorage.GetAllClients();
            Assert.Contains(storedClients, c => c.Key.FullName == client.FullName);
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
            
            Assert.Throws<UnderAgeClientException>(() => _clientService.AddClient(client));
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
            
            Assert.Throws<MissingPassportException>(() => _clientService.AddClient(client));
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
            _clientService.AddClient(client);

            Account newAccount = new Account
            {
                Amount = 100,
                Currency = new Currency { CurrencyName = "EUR", Symbol = "€" }
            };
            
            _clientService.AddAccountToClient(client, newAccount);
            
            var storedClient = _clientStorage.GetAllClients().FirstOrDefault(c => c.Key.PasNumber == client.PasNumber);
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
            _clientService.AddClient(client);
            
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
            
            _clientService.EditAccount(client, newAccount);
            
            var storedClient = _clientStorage.GetAllClients().FirstOrDefault(c => c.Key.PasNumber == client.PasNumber);
            Account updatedAccount = storedClient.Value.FirstOrDefault(a => a.Currency.CurrencyName == "RUB");
            Assert.Equal(500, updatedAccount.Amount);
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

            _clientService.AddClient(client1);
            _clientService.AddClient(client2);
            _clientService.AddClient(client3);
            
            List<Client> filteredClients = _clientService.GetFilteredClients("Alice", null, null, null, null);
            
            Assert.Single(filteredClients);
            Assert.Contains(filteredClients, c => c.FullName == "Alice Johnson");
            
            filteredClients = _clientService.GetFilteredClients(null, "555", null, 18, null);
            
            Assert.Single(filteredClients);
            Assert.Contains(filteredClients, c => c.FullName == "Charlie Brown");
            
            filteredClients = _clientService.GetFilteredClients(null, null, "B87654321", 20, 30);
            
            Assert.Single(filteredClients);
            Assert.Contains(filteredClients, c => c.FullName == "Bob Smith");
        }
}