﻿using System.Diagnostics;
using System.Diagnostics.Contracts;
using BankSystem.App.Services;
using BankSystem.Models;

public class Program
{
    static TestDataGenerator testDataGenerator = new TestDataGenerator();
    static BankService bankService = new BankService();
    public static List<Employee> employees = testDataGenerator.EmployeesList();
    
    public static void Main(string[] args) 
    {
        List<Client> clients = testDataGenerator.ClientsList();
        Dictionary<string, Client> clientsPhone = testDataGenerator.ClientsDictionary() ;

        Client client = new Client
        {
            FullName = "Tom Holland",
            PhoneNumber = "08098098",
            PasNumber = "1ПР-12412412",
            Age = 18
        };
        Currency currency = new Currency(){CurrencyName = "Usd", Symbol = "$"};
        
        Employee employee = bankService.ClientToEmployee(client);
        bankService.CalculateOwnerSalary(employees);
        
        ContractUpdate(employees);
        CurrencyUpdate(currency);

        Random random = new Random();
        string phoneToFind = clients[random.Next(clients.Count)].PhoneNumber;
        Stopwatch stopwatch = new Stopwatch();
        Client foundClient;

        for (int i = 0; i < 100; i++)
        {
            stopwatch.Start();
        
             foundClient = clients.Find(client => client.PhoneNumber == phoneToFind);
        
            stopwatch.Stop();
            stopwatch.Reset();
        }

        for (int i = 0; i < 100; i++)
        {
            stopwatch.Start();
            
            foundClient = clientsPhone[phoneToFind];
            
            stopwatch.Stop();
            stopwatch.Reset();
        }
        

        List<Client> clientsUnderAge = clients.Where(clnt => clnt.Age < 19).ToList();
        
        var employeeWithMinSalary = employees.OrderBy(empl => empl.Salary).First();

        for (int i = 0; i < 100; i++)
        {
            stopwatch.Start();
        
            clientsPhone.LastOrDefault();
        
            stopwatch.Stop();
            stopwatch.Reset();   
        }
        string number = clientsPhone.LastOrDefault().Value.PhoneNumber;

        for (int i = 0; i < 100; i++)
        {
            
        
            stopwatch.Start();
            client = clientsPhone[number];
            stopwatch.Stop();
            stopwatch.Reset();
        }
        
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