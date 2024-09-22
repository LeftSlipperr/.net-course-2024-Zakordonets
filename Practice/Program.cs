using System.Diagnostics.Contracts;
using BankSystem.App.Services;
using BankSystem.Models;

public class Program
{
    private static void Main(string[] args) 
    {
        List<Employee> employees = new List<Employee>
        {
            new Employee() { FirstName = "John", SecondName = "Bobson", Sallary = 8000, IsOwner = false, PasNumber = "1ПР-12412414", Number = 0435034590},
            new Employee() { FirstName = "Bob", SecondName = "Johnson", IsOwner = true, PasNumber = "1ПР-18275412", Number = 0943853045},
            new Employee() { FirstName = "Tom", SecondName = "Cruise", IsOwner = true, PasNumber = "1ПР-389472934", Number = 948390560}
        };

        Client client = new Client() { FirstName = "Tom", SecondName = "Holland", Number = 0884949384, PasNumber = "1ПР-12412412" };
        
        
        Currency currency = new Currency(){CurrencyName = "Usd", Symbol = "$"};

        BankService bankService = new BankService();
        Employee employee = bankService.ClientToEmployee(client);
        bankService.CalculateOwnerSalary(employees);

        ContractUpdate(employees);
        CurrencyUpdate(currency);
    }
        
    private static void ContractUpdate(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            employee.Contract = $"Контракт для {employee.FirstName} {employee.SecondName}, Должность: {employee.IsOwner}, Зарплата: {employee.Sallary} руб.";
        }
        
    }

    private static void CurrencyUpdate(Currency currency)
    {
        currency.CurrencyName = "Rup";
        currency.Symbol = "R";
    }
}