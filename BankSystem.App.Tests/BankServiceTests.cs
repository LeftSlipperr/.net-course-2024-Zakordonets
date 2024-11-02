using BankSystem.App.Services;
using BankSystem.Models;

namespace BankSystem.App.Tests;

public class BankServiceTests
{
    private readonly BankService _bankService;

    public BankServiceTests()
    {
        _bankService = new BankService();
    }

    [Fact]
    public void AddBonusToClientAddsBonusSuccessfully()
    {
        Client client = new Client
        {
            Name = "John",
            SecondName = "Bobson",
            Bonus = 100
        };
        
        _bankService.AddBonus(client, 50);

        Assert.Equal(150, client.Bonus);
    }

    [Fact]
    public void AddBonusToEmployeeAddsBonusSuccessfully()
    {
        Employee employee = new Employee
        {
            Name = "Jane",
            SecondName = "Doe", 
            Bonus = 200
        };

        _bankService.AddBonus(employee, 100);

        Assert.Equal(300, employee.Bonus);
    }

    [Fact]
    public void AddToBlackListAddsPersonSuccessfully()
    {
        Client client = new Client
        {
            Name = "John",
            SecondName = "Doe"
        };

        _bankService.AddToBlackList(client);

        Assert.True(_bankService.IsPersonInBlackList(client));
    }

    [Fact]
    public void AddToBlackListThrowsExceptionIfPersonAlreadyInBlackList()
    {
        Client client = new Client
        {
            Name = "John",
            SecondName = "Doe"
        };
        _bankService.AddToBlackList(client);
        
        var exception = Assert.Throws<Exception>(() => _bankService.AddToBlackList(client));
        Assert.Equal("Сущность уже в черном списке", exception.Message);
    }

    [Fact]
    public void IsPersonInBlackList_ReturnsTrueIfPersonIsInBlackList()
    {
        Client client = new Client
        {
            Name = "John Doe"
        };
        _bankService.AddToBlackList(client);

        bool isInBlackList = _bankService.IsPersonInBlackList(client);

        Assert.True(isInBlackList);
    }

    [Fact]
    public void IsPersonInBlackList_ReturnsFalseIfPersonIsNotInBlackList()
    {
        Client client = new Client
        {
            Name = "John",
            SecondName = "Doe"
        };

        bool isInBlackList = _bankService.IsPersonInBlackList(client);

        Assert.False(isInBlackList);
    }
}