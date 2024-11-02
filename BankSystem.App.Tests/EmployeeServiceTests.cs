using AutoMapper;
using BankSystem.App.DTO;
using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Infrastructure;
using BankSystem.Models;
using ClientStorage;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.App.Tests;

public class EmployeeServiceTests
{
    private readonly IEmployeeStorage _employeeStorage;
    private readonly EmployeeService _employeeService;
    private readonly TestDataGenerator _dataGenerator;
    private IMapper _mapper;

    public EmployeeServiceTests()
    {
        var options = new DbContextOptionsBuilder<BankSystemDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _employeeStorage = new EmployeeStorage(new BankSystemDbContext(options));
        _employeeService = new EmployeeService(_employeeStorage, _mapper);
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
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            await _employeeService.AddEmployeeAsync(employeeDto);
            
            List<Employee> employees = await _employeeStorage.GetAsync(employee.Id);
            Assert.Contains(employees, e => e.Name == employee.Name);
        }

        [Fact]
        public async Task AddEmployeeThrowsUnderAgeException()
        {
            Employee employee = new Employee()
            {
                Id = new Guid(),
                Name = "John",
                SecondName = "Bobson",
                ThirdName = "Bibson",
                Age = 17,
                PasNumber = "123456789",
                PhoneNumber = "1234567",
                IsOwner = false,
                Contract = "Контракт заключен",
                Salary = 20000
            };
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            await Assert.ThrowsAsync<UnderAgeClientException>(async () => await _employeeService.AddEmployeeAsync(employeeDto));
        }

        [Fact]
        public async Task EditEmployeeUpdatesEmployeeSuccessfully()
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
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            await _employeeService.AddEmployeeAsync(employeeDto);

            Employee updatedEmployee = new Employee()
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
            EmployeeDto updatedEmployeeDto = _mapper.Map<EmployeeDto>(employee);
            await _employeeService.UpdateEmployeeAsync(updatedEmployee.Id, updatedEmployeeDto);

            var storedEmployee = await _employeeStorage.GetAsync(updatedEmployee.Id);

            Assert.Equal(updatedEmployee.Id, storedEmployee.FirstOrDefault().Id);
        }
        
        [Fact]
        public async Task GetEmployeeSuccessfully()
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

            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            await _employeeService.AddEmployeeAsync(employeeDto);
            
            List<Employee> filteredClients = await _employeeService.GetAsync(employee);
            
            Assert.Contains(filteredClients, c => c.Name == "John");
        }
}