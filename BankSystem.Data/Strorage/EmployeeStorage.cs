using BankSystem.Models;

namespace ClientStorage;

public class EmployeeStorage
{
    private List<Employee> _employees = new List<Employee>();
    
    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }
    
    public Employee GetYoungestEmployee()
    {
        return _employees.OrderBy(e => e.Age).FirstOrDefault();
    }
    
    public Employee GetOldestEmployee()
    {
        return _employees.OrderByDescending(e => e.Age).FirstOrDefault();
    }
    
    public double GetAverageAge()
    {
        return _employees.Average(e => e.Age);
    }
}