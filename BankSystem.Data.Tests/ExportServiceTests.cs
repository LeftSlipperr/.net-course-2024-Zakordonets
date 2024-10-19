using System.Globalization;
using BankSystem.Models;
using ClientStorage;
using CsvHelper;
using CsvHelper.Configuration;
using ExportTool;
using Xunit;

namespace BankSystem.Data.Tests;

public class ExportServiceTests
{
        [Fact]
        public void WriteClientsToCsvSuccessfully()
        {
            ExportService exportService = new ExportService();
            Infrastructure.ClientStorage clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
            
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
            Client client2 = new Client
            {
                Id = new Guid(),
                Name = "Bob",
                SecondName = "Johnson",
                ThirdName = "Bibson",
                Age = 25,
                PasNumber = "123456789",
                PhoneNumber = "1234567",
                AccountNumber = 123,
                Balance = 123
            };
        
            clientStorage.Add(client);
            clientStorage.Add(client2);

            var clients = clientStorage.GetListOfclients();
            
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "test.csv");
            
            exportService.WriteClientsToCsv();
            
            Assert.True(File.Exists(filePath));
            
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var readClients = csv.GetRecords<Client>().ToList();
                Assert.Equal(clients.Count, readClients.Count);
                Assert.Equal(clients[0].Name, readClients[0].Name);
                Assert.Equal(clients[1].Name, readClients[1].Name);
            }
        }
        
        [Fact]
        public void ReadClientsFromCsvSuccessfully()
        {
            Infrastructure.ClientStorage clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
            
            var exportService = new ExportService();
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "test.csv");
            
            var clients = clientStorage.GetListOfclients();

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(clients);
            }
            
            var readClients = exportService.ReadClientsFromCsv();
            
            Assert.Equal(clients.Count, readClients.Count);
            Assert.Equal(clients[0].Name, readClients[0].Name);
            Assert.Equal(clients[1].Name, readClients[1].Name);
        }
}