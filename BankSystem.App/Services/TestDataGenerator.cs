using System.Diagnostics.Contracts;
using System.Security.Authentication;
using BankSystem.Models;
using Bogus;
using Bogus.DataSets;

namespace BankSystem.App.Services;

public class TestDataGenerator
{
    
    private List<Client> clients = new List<Client>();
    private List<Employee> employees = new List<Employee>();
    private Dictionary<string, Client> clientsPhone = new Dictionary<string, Client>();
    Faker faker = new Faker("ru");

    public List<Client> ClientsList()
    {
        for (int i = 0; i < 1000; i++)
        {
            clients.Add(new Client{FullName = faker.Name.FullName(),
                PhoneNumber = faker.Phone.PhoneNumber() + i.ToString(),
                PasNumber = "1ПР " + faker.Random.String2(8, 8, "123456789"),
                Age = faker.Random.Int(1, 90)});
        }
        return clients;
    }

    public Dictionary<string,Client> ClientsDictionary()
    {
        foreach (var client in clients)
        {
            clientsPhone.Add(client.PhoneNumber, client);
        }

        return clientsPhone;
    }

    public List<Employee> EmployeesList()
    {

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
}