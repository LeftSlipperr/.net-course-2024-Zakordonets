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
            owner.Salary = Salary;
        }
    }

    public Employee ClientToEmployee(Client client)
    {
        Employee employee = new Employee(client.FullName.ToString(), false, 80000, client.Number.ToString(), client.PasNumber.ToString());
        return employee;
    }
}