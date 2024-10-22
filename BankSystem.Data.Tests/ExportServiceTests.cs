using System.Globalization;
using BankSystem.App.Services;
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

            var clients = clientStorage.GetClientsByParameters("John");
            
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "test.csv");
            
            exportService.WriteClientsToCsv(clients, filePath);
            
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var readClients = csv.GetRecords<Client>().ToList();
                Assert.Equal(clients.Count, readClients.Count);
                Assert.Equal(clients[0].Name, readClients[0].Name);
                Assert.Equal(clients[1].Name, readClients[1].Name);
            }
            
            Assert.True(File.Exists(filePath));
        }
        
        [Fact]
        public void ReadClientsFromCsvSuccessfully()
        {
            Infrastructure.ClientStorage clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
            
            var exportService = new ExportService();
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "test.csv");
            
            var clients = clientStorage.GetClientsByParameters("John");

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(clients);
            }
            
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
        public void SerializeClientsSuccessfully()
        {
            Infrastructure.ClientStorage clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
            var exportService = new ExportService();
            
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "client.json");
            exportService.ItemsSerialization(clientStorage.GetClientsByParameters("John"), filePath);
            
            string jsonFromFile = File.ReadAllText(filePath);
            Assert.Contains("John", jsonFromFile);
            
        }
        
        [Fact]
        public void SerializeEmployeesSuccessfully()
        {
            Infrastructure.EmployeeStorage employeeStorage = new Infrastructure.EmployeeStorage(new BankSystemDbContext());
            var exportService = new ExportService();
            
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "employee.json");
            exportService.ItemsSerialization(employeeStorage.GetEmployeesByParameters("John"), filePath);
            
            string jsonFromFile = File.ReadAllText(filePath);
            Assert.Contains("John", jsonFromFile);
        }

        [Fact]
        public void DeserializeEmployeesSuccessfully()
        {
            Infrastructure.EmployeeStorage employeeStorage = new Infrastructure.EmployeeStorage(new BankSystemDbContext());
            var exportService = new ExportService();

            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "employee.json");
            
            var employeesDeserialize = exportService.ItemsDeserialization<Employee>(filePath);
            
            foreach (var employee in employeesDeserialize)
                employeeStorage.Add(employee);

            Assert.NotNull(employeesDeserialize); 
            Assert.NotEmpty(employeesDeserialize);
            Assert.Contains(employeesDeserialize, e => e.Name == "John");
        }
        
        [Fact]
        public void DeserializeClientsSuccessfully()
        {
            Infrastructure.ClientStorage clientStorage = new Infrastructure.ClientStorage(new BankSystemDbContext());
            var exportService = new ExportService();

            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "client.json");
            
            var clientDeserialize = exportService.ItemsDeserialization<Client>(filePath);

            foreach (var client in clientDeserialize)
            {
                clientStorage.Add(client);
            }

            Assert.NotNull(clientDeserialize); 
            Assert.NotEmpty(clientDeserialize);
            Assert.Contains(clientDeserialize, c => c.Name == "John");
        }
}