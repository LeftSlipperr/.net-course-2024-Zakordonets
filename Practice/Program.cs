using System.Diagnostics;
using System.Diagnostics.Contracts;
using BankSystem.App.Services;
using BankSystem.Models;

public class Program
{
    static TestDataGenerator testDataGenerator = new TestDataGenerator();
    static BankService bankService = new BankService();
    public static List<Employee> employees = testDataGenerator.GenerateEmployee();
    
    public static void Main(string[] args) 
    {
        List<Client> clients = testDataGenerator.GenerateClient();
        Dictionary<string, Client> clientsPhone = testDataGenerator.GenerateDictionaryClient();
        
        Client client = new Client("Tom Holland", "08098098", "1ПР-12412412", 18) {  };
        Currency currency = new Currency(){CurrencyName = "Usd", Symbol = "$"};
        
        Employee employee = bankService.ClientToEmployee(client);
        bankService.CalculateOwnerSalary(employees);
        
        ContractUpdate(employees);
        CurrencyUpdate(currency);

        Random random = new Random();
        string phoneToFind = clients[random.Next(clients.Count)].Number;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        Client foundClient = clients.Find(client => client.Number == phoneToFind);
        
        stopwatch.Stop();
        stopwatch.Reset();
        
        stopwatch.Start();
        foundClient = clientsPhone[phoneToFind];
        stopwatch.Stop();

        List<Client> clientsUnderAge = clients.Where(clnt => clnt.Age < 19).ToList();
        
        var employeeWithMinSalary = employees.OrderBy(empl => empl.Salary).First();
        
        stopwatch.Start();
        clientsPhone.FirstOrDefault();
        stopwatch.Stop();
        stopwatch.Reset();

        string number = clientsPhone.FirstOrDefault().Value.Number;
        
        stopwatch.Start();
        client = clientsPhone[number];
        stopwatch.Stop();
    }
        
    public static void ContractUpdate(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            employee.Contract = $"Контракт для {employee.FullName}, Должность: {employee.IsOwner}, Зарплата: {employee.Salary} руб.";
        }
        
    }

    public static void CurrencyUpdate(Currency currency)
    {
        currency.CurrencyName = "Rup";
        currency.Symbol = "R";
    }
}