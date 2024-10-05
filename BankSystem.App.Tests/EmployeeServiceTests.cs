using BankSystem.App.Services;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;
using ClientStorage;

namespace BankSystem.App.Tests;

public class EmployeeServiceTests
{
        private EmployeeService _employeeService;
        private EmployeeStorage _employeeStorage;

        public EmployeeServiceTests()
        {
            _employeeStorage = new EmployeeStorage();
            _employeeService = new EmployeeService(_employeeStorage);
        }

        [Fact]
        public void AddEmployee_AddsEmployeeSuccessfully()
        {
            Employee employee = new Employee
            {
                FullName = "John Bobson",
                Age = 30,
                PasNumber = "1234567"
            };
            
            _employeeService.AddEmployee(employee);
            
            List<Employee> employees = _employeeStorage.GetAllEmployees();
            Assert.Contains(employees, e => e.FullName == employee.FullName);
        }

        [Fact]
        public void AddEmployee_ThrowsUnderAgeException()
        {
            Employee employee = new Employee
            {
                FullName = "John Bobson",
                Age = 16,
                PasNumber = "1234567"
            };

            Assert.Throws<UnderAgeClientException>(() => _employeeService.AddEmployee(employee));
        }

        [Fact]
        public void AddEmployee_ThrowsMissingPassportException()
        {
            Employee employee = new Employee
            {
                FullName = "Bob Johnson",
                Age = 25,
                PasNumber = ""
            };

            Assert.Throws<MissingPassportException>(() => _employeeService.AddEmployee(employee));
        }

        [Fact]
        public void EditEmployee_UpdatesEmployeeSuccessfully()
        {
            Employee employee = new Employee
            {
                FullName = "John Bobson",
                Age = 30,
                PasNumber = "1234567"
            };
            
            _employeeService.AddEmployee(employee);

            Employee updatedEmployee = new Employee
            {
                FullName = "John Bobson",
                Age = 35,
                Salary = 50000,
                PhoneNumber = "123-456-7890",
                Contract = "Full-Time"
            };
            
            _employeeService.EditEmployee(employee.PasNumber, updatedEmployee);

            var storedEmployee = _employeeStorage.GetAllEmployees()
                .FirstOrDefault(e => e.PasNumber == employee.PasNumber);

            Assert.Equal(35, storedEmployee.Age);
            Assert.Equal(50000, storedEmployee.Salary);
            Assert.Equal("123-456-7890", storedEmployee.PhoneNumber);
            Assert.Equal("Full-Time", storedEmployee.Contract);
        }
        
        [Fact]
        public void FilterClientsReturnsFilteredClients()
        {
            Employee employee1 = new Employee()
            {
                FullName = "Alice Johnson",
                Age = 30,
                PasNumber = "A12345678",
                PhoneNumber = "1234567890"
            };

            Employee employee2 = new Employee()
            {
                FullName = "Bob Smith",
                Age = 25,
                PasNumber = "B87654321",
                PhoneNumber = "0987654321"
            };

            Employee employee3 = new Employee()
            {
                FullName = "Charlie Brown",
                Age = 20,
                PasNumber = "C12398745",
                PhoneNumber = "5555555555"
            };

            _employeeService.AddEmployee(employee1);
            _employeeService.AddEmployee(employee2);
            _employeeService.AddEmployee(employee3);
            
            List<Employee> filteredClients = _employeeService.GetFilterEmployees("Alice", null, null, null, null);
            
            Assert.Single(filteredClients);
            Assert.Contains(filteredClients, c => c.FullName == "Alice Johnson");
            
            filteredClients = _employeeService.GetFilterEmployees(null, "555", null, 18, null);
            
            Assert.Single(filteredClients);
            Assert.Contains(filteredClients, c => c.FullName == "Charlie Brown");
            
            filteredClients = _employeeService.GetFilterEmployees(null, null, "B87654321", 20, 30);
            
            Assert.Single(filteredClients);
            Assert.Contains(filteredClients, c => c.FullName == "Bob Smith");
        }
}