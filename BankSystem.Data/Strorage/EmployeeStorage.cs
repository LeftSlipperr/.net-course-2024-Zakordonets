using BankSystem.Models;

namespace ClientStorage;

public class EmployeeStorage
{
    private List<Employee> employees = new List<Employee>();
    
    public void AddEmployee(Employee employee)
    {
        employees.Add(employee);
    }
    
    public Employee GetYoungestEmployee()
    {
        return employees.OrderBy(e => e.Age).FirstOrDefault();
    }
    
    public Employee GetOldestEmployee()
    {
        return employees.OrderByDescending(e => e.Age).FirstOrDefault();
    }
    
    public double GetAverageAge()
    {
        return employees.Average(e => e.Age);
    }
    
    public List<Employee> GetAllEmployees()
    {
        return employees;
    }
}