using BankSystem.App.Services;
using BankSystem.Models;

namespace BankSystem.App.Tests;

public class EquivalenceTests
{
    [Fact]
    public void GetHashCodeNecessityPositivTest()
    {
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        List<Client> clients = testDataGenerator.ClientsList();
        Dictionary<Client, List<Account>> clientsAccont= testDataGenerator.ClientsDictionary();
        
        Client client = new Client();
        client = clients.LastOrDefault();
        List<Account> account = clientsAccont[client];
        
        Assert.Equal(account, clientsAccont[clients.LastOrDefault()]);
    }
    
    [Fact]
    public void EmployeeNecessityPositiveTest()
    {
        TestDataGenerator testDataGenerator = new TestDataGenerator();
        List<Employee> employees = testDataGenerator.EmployeesList();
        Dictionary<Employee, List<Account>> employeesAccount = testDataGenerator.EmployeesDictionary();
        
        Employee employee = new Employee(){FullName = employees.LastOrDefault().FullName, PhoneNumber = employees.LastOrDefault().PhoneNumber, Age = employees.LastOrDefault().Age, PasNumber = employees.LastOrDefault().PasNumber};
        List<Account> account = employeesAccount[employee];
        
        Assert.Equal(account, employeesAccount[employees.LastOrDefault()]);
    }
}