using System.Diagnostics.Contracts;
using BankSystem.Models;

public class Program
{
    private static void Main(string[] args)
    {
        Employee employee = new Employee(){FirstName = "John", SecondName = "Bobson", Sallary = 8000};
        Currency currency = new Currency(){CurrencyName = "Usd", Symbol = "$"};
        
        ContractUpdate(employee);
        CurrencyUpdate(currency);
    }
        
    private static void ContractUpdate(Employee employee)
    {
        employee.Contract = "Заключен контракт";
    }

    private static void CurrencyUpdate(Currency currency)
    {
        currency.CurrencyName = "Rup";
        currency.Symbol = "R";
    }
}