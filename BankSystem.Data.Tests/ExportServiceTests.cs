using System.Globalization;
using BankSystem.App.Services;
using BankSystem.Models;
using ClientStorage;
using CsvHelper;
using CsvHelper.Configuration;
using ExportTool;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BankSystem.Data.Tests;

public class ExportServiceTests
{
    private ExportService exportService;
    private Storage.ClientStorage clientStorage;
    private Infrastructure.EmployeeStorage employeeStorage;

    public ExportServiceTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<BankSystemDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Username=postgres;Password=mysecretpassword;Database=local");
        
        var dbContext = new BankSystemDbContext(optionsBuilder.Options);
        
        clientStorage = new Storage.ClientStorage(dbContext); 
        employeeStorage = new Infrastructure.EmployeeStorage(dbContext);
        
        exportService = new ExportService(clientStorage);
    }
    
    [Fact]
    public async Task WriteClientsToCsvSuccessfully()
    {
        Client client = new Client
        {
            Id = Guid.NewGuid(),
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
            Id = Guid.NewGuid(),
            Name = "Bob",
            SecondName = "Johnson",
            ThirdName = "Bibson",
            Age = 25,
            PasNumber = "123456789",
            PhoneNumber = "1234567",
            AccountNumber = 123,
            Balance = 123
        };
    
        await clientStorage.AddAsync(client);
        await clientStorage.AddAsync(client2);

        var clients = await clientStorage.GetClientsByParametersAsync("John");
        
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
        public async Task ReadClientsFromCsvSuccessfully()
        {
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "test.csv");
            
            var clients = await clientStorage.GetClientsByParametersAsync("John");

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
        public async Task SerializeClientsSuccessfully()
        {
            
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
            
            await clientStorage.AddAsync(client);
            
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "client.json");
            var clientSerialize = await clientStorage.GetClientsByParametersAsync("John");
            exportService.ItemsSerialization(clientSerialize.FirstOrDefault(), filePath);
            
            string jsonFromFile = File.ReadAllText(filePath);
            Assert.Contains("John", jsonFromFile);
            
        }
        
        [Fact]
        public async Task SerializeEmployeesSuccessfully()
        {
            
            Employee employee = new Employee()
            {
                Id = new Guid(),
                Name = "John",
                SecondName = "Bobson",
                ThirdName = "Bibson",
                Age = 25,
                PasNumber = "123456789",
                PhoneNumber = "1234567",
                IsOwner = false,
                Contract = "Контракт заключен",
                Salary = 20000
            };
        
            await employeeStorage.AddAsync(employee);
            
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "employee.json");
            var serializeEmployee = await employeeStorage.GetEmployeesByParameters("John");
            exportService.ItemsSerialization(serializeEmployee.FirstOrDefault(), filePath);
            
            string jsonFromFile = File.ReadAllText(filePath);
            Assert.Contains("John", jsonFromFile);
        }
        
        [Fact]
        public async Task DeserializeEmployeeSuccessfully()
        {

            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "employee.json");
            
            var employeesDeserialize = exportService.ItemsDeserialization<Employee>(filePath);
            
            await employeeStorage.DeleteAsync(employeesDeserialize.FirstOrDefault().Id);
            await employeeStorage.AddAsync(employeesDeserialize.FirstOrDefault());

            Assert.NotNull(employeesDeserialize); 
            Assert.Equal( "John", employeesDeserialize.FirstOrDefault().Name);
        }
        
        [Fact]
        public async Task DeserializeClientSuccessfully()
        {
            string filePath = Path.Combine("C:", "Users", "Admin", "Desktop", "client.json");
            
            var clientDeserialize = exportService.ItemsDeserialization<Client>(filePath);

            await clientStorage.DeleteAsync(clientDeserialize.FirstOrDefault().Id);
            await clientStorage.AddAsync(clientDeserialize.FirstOrDefault());
            

            Assert.NotNull(clientDeserialize); 
            Assert.Equal("John", clientDeserialize.FirstOrDefault().Name);
        }
}