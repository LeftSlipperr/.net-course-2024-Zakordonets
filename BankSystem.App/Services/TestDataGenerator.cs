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

    public List<Client> GenerateClient()
    {
        for (int i = 0; i < 1000; i++)
        {
            clients.Add(new Client(faker.Name.FullName(), faker.Phone.PhoneNumber()+i.ToString(), "1ПР "+faker.Random.String2(8,8,"123456789"), faker.Random.Int(1,90)));
        }
        return clients;
    }

    public Dictionary<string,Client> GenerateDictionaryClient()
    {
        foreach (var client in clients)
        {
            clientsPhone.Add(client.Number, client);
        }

        return clientsPhone;
    }

    public List<Employee> GenerateEmployee()
    {

        for (int i = 0; i < 1000; i++)
        {
            employees.Add(new Employee(faker.Name.FullName(), faker.Random.Bool(1), faker.Random.Int(1000, 10000),
                faker.Phone.PhoneNumber() + i.ToString(), "1ПР " + faker.Random.String2(8, 8, "123456789")));
        }

        return employees;
    }
}