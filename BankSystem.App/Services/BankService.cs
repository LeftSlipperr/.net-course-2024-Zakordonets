using BankSystem.Models;

namespace BankSystem.App.Services;

public class BankService
{
    private List<Person> _blackList = new List<Person>();
    
    public void CalculateOwnerSalary(List<Employee> employees)
    {
        var owners = employees.Where(e => e.IsOwner).ToList();
        int Salary = (40000 - 10000) / owners.Count;
        foreach (var owner in owners)
        {
            owner.Salary = Salary;
        }
    }

    public Employee ClientToEmployee(Client client)
    {
        Employee employee = new Employee()
        {
            Name = client.Name, 
            PhoneNumber = client.PhoneNumber,
            PasNumber = client.PasNumber
        };

        return employee;
    }

    public void AddBonus(Person person, decimal bonus)
    {
        if (person is Client client)
        {
            client.Bonus += bonus;
        } else if (person is Employee employee)
        {
            employee.Bonus += bonus;
        }
        else
            throw new Exception("Сущность не является ни клиентом ни работником");
    }
    
    public void AddToBlackList<T>(T person) where T : Person
    {
        if (!_blackList.Contains(person))
        {
            _blackList.Add(person);
        }
        else
        {
            throw new Exception("Сущность уже в черном списке");
        }
    }
    
    public bool IsPersonInBlackList<T>(T person) where T : Person
    {
        bool isInBlackList = _blackList.Contains(person);
        return isInBlackList;
    }
}