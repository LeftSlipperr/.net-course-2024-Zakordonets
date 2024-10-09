using BankSystem.App.Interfaces;
using BankSystem.Models;

namespace BankSystem.Infrastructure;

public class EmployeeStorage : IStorage<Employee, List<Employee>>
{
    private List<Employee> _employees;
    
    public EmployeeStorage()
    {
        _employees = new List<Employee>();
    }
    
    public List<Employee> Get(Func<Employee, bool> filter)
    {
        return _employees.Where(filter).ToList();
    }

    public void Add(Employee employee)
    {
        _employees.Add(employee);
    }

    public void Update(Employee employee)
    {
        var newEmployee = _employees
            .FirstOrDefault(e => e.PasNumber == employee.PasNumber);

        newEmployee.FullName = employee.FullName;
        newEmployee.Age = employee.Age;
        newEmployee.PasNumber = employee.PasNumber;
        newEmployee.PhoneNumber = employee.PhoneNumber;
        newEmployee.Contract = employee.Contract;
        newEmployee.Salary = employee.Salary;
        newEmployee.IsOwner = employee.IsOwner;
    }

    public void Delete(Employee employee)
    {
        _employees.Remove(employee);
    }

    public List<Employee> GetAllEmployees()
    {
        return _employees;
    }
}