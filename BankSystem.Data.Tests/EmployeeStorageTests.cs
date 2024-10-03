using BankSystem.App.Services;
using BankSystem.Models;
using ClientStorage;
using Xunit;

namespace BankSystem.Data.Tests;

public class EmployeeStorageTests
{
        EmployeeStorage employeeStorage = new EmployeeStorage();
        static TestDataGenerator testDataGenerator = new TestDataGenerator();
        List<Employee> employees = testDataGenerator.EmployeesList();

        [Fact]
        public void AddEmployeeAddsEmployeeSuccessfully()
        {
            foreach (var employee in employees)
            {
                employeeStorage.AddEmployee(employee);
            }

            Assert.Equal(1000, employeeStorage.GetAllEmployees().Count);
        }
        
        [Fact]
        public void GetYoungestEmployeeReturnsCorrectEmployee()
        {
            foreach (var employee in employees)
            {
                employeeStorage.AddEmployee(employee);
            }

            Employee youngestEmployee = employees.OrderBy(e => e.Age).FirstOrDefault();
            Employee youngest = employeeStorage.GetYoungestEmployee();

            Assert.Equal(youngest.FullName, youngestEmployee.FullName);
        }

        [Fact]
        public void GetOldestEmployeeReturnsCorrectClient()
        {
            foreach (var employee in employees)
            {
                employeeStorage.AddEmployee(employee);
            }

            Employee oldestEmployee = employees.OrderByDescending(e => e.Age).FirstOrDefault();
            Employee oldest = employeeStorage.GetOldestEmployee();

            Assert.Equal(oldest.FullName, oldestEmployee.FullName);
        }

        [Fact]
        public void GetAverageAgeReturnsCorrectValue()
        {
            foreach (var employee in employees)
            {
                employeeStorage.AddEmployee(employee);
            }

            double avgAgeEmployees = employees.Average(e => e.Age);
            double avgAge = employeeStorage.GetAverageAge();

            Assert.Equal(avgAge, avgAgeEmployees);
        }
}