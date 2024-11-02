using BankSystem.App.Services;
using BankSystem.Models;
using BankSystem.Infrastructure;
using ClientStorage;
using Xunit;

namespace BankSystem.Data.Tests;

public class EmployeeStorageTests
{
        static TestDataGenerator testDataGenerator = new TestDataGenerator();
        private List<Employee> employees = testDataGenerator.EmployeesList();
        private readonly EmployeeStorage _employeeStorage;
        private readonly TestDataGenerator _dataGenerator;

        public EmployeeStorageTests()
        {
            _employeeStorage = new EmployeeStorage(new BankSystemDbContext());
            _dataGenerator = new TestDataGenerator();
        }   

        [Fact]
        public async Task AddEmployeeAddsEmployeeSuccessfully()
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
            
            await _employeeStorage.AddAsync(employee);

            employees = await _employeeStorage.GetAsync(employee.Id);
            var myEmployee = employees.FirstOrDefault(e => e.Id == employee.Id); 

            Assert.Equal(myEmployee.Id, employee.Id);
        }
        
    [Fact]
    public async Task UpdateEmployeetPositiveTest()
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
        
        await _employeeStorage.AddAsync(employee);
        
        Employee employee2 = new Employee()
        {
            Id = employee.Id,
            Name = "Updated",
            SecondName = "Bobson",
            ThirdName = "Bibson",
            Age = 25,
            PasNumber = "123456789",
            PhoneNumber = "1234567",
            IsOwner = false,
            Contract = "Контракт заключен",
            Salary = 20000
        };
        
        await _employeeStorage.UpdateAsync(employee2);
        
        employees = await _employeeStorage.GetAsync(employee.Id);
        var myEmployee = employees.FirstOrDefault(e => e.Id == employee.Id); 

        Assert.Equal(myEmployee.Id, employee2.Id);
    }
    
    
    [Fact]
    public async Task DeleteEmployeePositiveTest()
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
        
        await _employeeStorage.AddAsync(employee);
        
        await _employeeStorage.DeleteAsync(employee.Id);
        
        var newEmployee = await _employeeStorage.GetAsync(employee.Id);
        var deletedEmployee = newEmployee.FirstOrDefault();
        
        Assert.Null(deletedEmployee);
    }
}