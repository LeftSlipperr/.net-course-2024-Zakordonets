using System.Diagnostics.Contracts;
using System.Security.Authentication;
using BankSystem.Models;
using Bogus;
using Bogus.DataSets;
using Currency = BankSystem.Models.Currency;

namespace BankSystem.App.Services;

public class TestDataGenerator
{
    
    private List<Client> clients = new List<Client>();
    private Dictionary<Client, List<Account>> clientsAccount = new Dictionary<Client, List<Account>>();
    private List<Employee> employees = new List<Employee>();
    private Dictionary<Employee, List<Account>> employeesAccount =  new Dictionary<Employee, List<Account>>();
    
    Faker faker = new Faker("ru");

    public List<Client> ClientsList()
    {
        clients = new List<Client>();
        for (int i = 0; i < 1000; i++)
        {
            clients.Add(new Client{FullName = faker.Name.FullName(),
                PhoneNumber = faker.Phone.PhoneNumber() + i.ToString(),
                PasNumber = "1ПР " + faker.Random.String2(8, 8, "123456789"),
                Age = faker.Random.Int(1, 90)
            });
        }
        return clients;
    }
    
    public Dictionary<Client,List<Account>> ClientsDictionary()
    {
        Random random = new Random();
        foreach (var client in clients)
        {
            List<Account> accounts = new List<Account>();
            for (int i = 0; i < random.Next(1,4); i++)
            {
                accounts.Add(new Account()
                {
                    Amount = faker.Finance.Amount(),
                    Currency = new Currency()
                    {
                        CurrencyName = "Rub", 
                        Symbol = "R"
                    }
                });
            }
            clientsAccount.Add(client, accounts);
        }

        return clientsAccount;
    }

    public List<Employee> EmployeesList()
    {
        employees = new List<Employee>();
        for (int i = 0; i < 1000; i++)
        {
            employees.Add(new Employee
            {
                FullName = faker.Name.FullName(),
                PhoneNumber = faker.Phone.PhoneNumber() + i.ToString(),
                PasNumber = "1ПР " + faker.Random.String2(8, 8, "123456789"),
                Age = faker.Random.Int(1, 90)
                
            });
        }

        return employees;
    }
    
    public Dictionary<Employee,List<Account>> EmployeesDictionary()
    {
        Random random = new Random();
        foreach (var employee in employees)
        {
            List<Account> accounts = new List<Account>();
            for (int i = 0; i < random.Next(1,4); i++)
            {
                accounts.Add(new Account()
                {
                    Amount = faker.Finance.Amount(),
                    Currency = new Currency()
                    {
                        CurrencyName = "Rub", 
                        Symbol = "R"
                    }
                });
            }
            
            
            employeesAccount.Add(employee, accounts);
        }

        return employeesAccount;
    }
}