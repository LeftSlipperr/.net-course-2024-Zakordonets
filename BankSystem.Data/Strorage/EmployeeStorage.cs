using BankSystem.Models;

namespace ClientStorage;

public class EmployeeStorage
{
    private List<Employee> _employees = new List<Employee>();
    
    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }

    public void UpdateEmployee(Employee employee, Employee updatedEmployee)
    {
        employee.FullName = updatedEmployee.FullName;
        employee.Age = updatedEmployee.Age;
        employee.Salary = updatedEmployee.Salary;
        employee.PhoneNumber = updatedEmployee.PhoneNumber;
        employee.Contract = updatedEmployee.Contract;
    }
    
    public List<Employee> GetFilterEmployees(string fullName, string phoneNumber, string pasNumber, int? minAge, int? maxAge)
    {
        return _employees
            .Where(e => 
                (string.IsNullOrEmpty(fullName) || e.FullName.Contains(fullName)) &&
                (string.IsNullOrEmpty(phoneNumber) || e.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrEmpty(pasNumber) || e.PasNumber.Contains(pasNumber)) &&
                (!minAge.HasValue || e.Age >= minAge.Value) &&
                (!maxAge.HasValue || e.Age <= maxAge.Value))
            .ToList();
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
    
    public List<Employee> GetAllEmployees()
    {
        return _employees;
    }
}