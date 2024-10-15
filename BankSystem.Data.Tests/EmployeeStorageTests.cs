using BankSystem.App.Services;
using BankSystem.Models;
using BankSystem.Infrastructure;
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
            _employeeStorage = new EmployeeStorage();
            _dataGenerator = new TestDataGenerator();
        }   

        [Fact]
        public void AddEmployeeAddsEmployeeSuccessfully()
        {
            foreach (var employee in employees)
            {
                _employeeStorage.Add(employee);
            }

            Assert.Equal(1000, _employeeStorage.GetAllEmployees().Count);
        }
        
         [Fact]
    public void UpdateClientPositiveTest()
    {
        Employee employee = new Employee();
        employee.FullName = "John Bobson";
        employee.PasNumber = "123";
        
        _employeeStorage.Add(employee);
        
        var updatedEmployee = new Employee();
        updatedEmployee.PasNumber = employee.PasNumber;
        updatedEmployee.FullName = "updatedFullName";
        updatedEmployee.PhoneNumber = employee.PhoneNumber;
        
        _employeeStorage.Update(updatedEmployee);
        
        employees = _employeeStorage.GetAllEmployees();
        Assert.Contains(updatedEmployee, employees);
    }
    
    
    [Fact]
    public void DeleteClientPositiveTest()
    {
        Employee employee = new Employee();
        employee.FullName = "John Bobson";
        employee.PasNumber = "123";
        
        _employeeStorage.Add(employee);
        
        _employeeStorage.Delete(employee);
        
        employees = _employeeStorage.GetAllEmployees();
        Assert.DoesNotContain(employee, employees);
    }
}