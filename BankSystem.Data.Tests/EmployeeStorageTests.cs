using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.Models;
using BankSystem.Infrastructure;
using ClientStorage;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BankSystem.Data.Tests;

public class EmployeeStorageTests : IDisposable
{
    private readonly EmployeeStorage _employeeStorage;
    private readonly BankSystemDbContext _context;
    private readonly TestDataGenerator _dataGenerator;

    public EmployeeStorageTests()
    {
        var options = new DbContextOptionsBuilder<BankSystemDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new BankSystemDbContext(options);
        _employeeStorage = new EmployeeStorage(_context);
        _dataGenerator = new TestDataGenerator();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task AddEmployeeAddsEmployeeSuccessfully()
    {
        Employee employee = new Employee()
        {
            Id = Guid.NewGuid(),
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

        var employees = await _employeeStorage.GetAsync(employee.Id);
        var myEmployee = employees.FirstOrDefault(e => e.Id == employee.Id);

        Assert.NotNull(myEmployee);
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
        
        await _employeeStorage.UpdateAsync(employee2.Id, employee2);
        
        var employees = await _employeeStorage.GetAsync(employee.Id);
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