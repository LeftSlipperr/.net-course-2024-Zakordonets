using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Infrastructure;
using BankSystem.Models;
using ClientStorage;

namespace BankSystem.App.Tests;

public class EmployeeServiceTests
{
        private readonly IStorage<Employee, List<Employee>> _employeeStorage;
        private readonly EmployeeService _employeeService;
        private readonly TestDataGenerator _dataGenerator;

        public EmployeeServiceTests()
        {
            _employeeStorage = new EmployeeStorage(new BankSystemDbContext());
            _employeeService = new EmployeeService(_employeeStorage);
            _dataGenerator = new TestDataGenerator();
        }

        [Fact]
        public void AddEmployeeAddsEmployeeSuccessfully()
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
            
            _employeeService.AddEmployee(employee);
            
            List<Employee> employees = _employeeStorage.Get(employee.Id);
            Assert.Contains(employees, e => e.Name == employee.Name);
        }

        [Fact]
        public void AddEmployeeThrowsUnderAgeException()
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

            Assert.Throws<UnderAgeClientException>(() => _employeeService.AddEmployee(employee));
        }

        [Fact]
        public void EditEmployeeUpdatesEmployeeSuccessfully()
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
            
            _employeeService.AddEmployee(employee);

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
            
            _employeeService.EditEmployee(updatedEmployee);

            var storedEmployee = _employeeStorage.Get(updatedEmployee.Id);

            Assert.Equal(updatedEmployee.Id, storedEmployee.FirstOrDefault().Id);
        }
        
        [Fact]
        public void GetEmployeeSuccessfully()
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

            
            _employeeService.AddEmployee(employee);
            
            List<Employee> filteredClients = _employeeService.Get(employee);
            
            Assert.Contains(filteredClients, c => c.Name == "John");
        }
}