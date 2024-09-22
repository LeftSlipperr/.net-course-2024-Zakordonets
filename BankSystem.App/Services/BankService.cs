using BankSystem.Models;

namespace BankSystem.App.Services;

public class BankService
{
    public void CalculateOwnerSalary(List<Employee> employees)
    {
        var owners = employees.Where(e => e.IsOwner).ToList();
        int Salary = (40000 - 10000) / owners.Count;
        foreach (var owner in owners)
        {
            owner.Sallary = Salary;
        }
    }

    public Employee ClientToEmployee(Client client)
    {
        Employee employee = new Employee()
        {
            FirstName = client.FirstName, 
            SecondName = client.SecondName, 
            Number = client.Number,
            PasNumber = client.PasNumber
        };

        return employee;
    }
}