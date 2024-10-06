using BankSystem.App.Services.Exceptions;
using BankSystem.Models;
using ClientStorage;

namespace BankSystem.App.Services;

public class EmployeeService
{
    private EmployeeStorage _employeeStorage;

        public EmployeeService(EmployeeStorage employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public void AddEmployee(Employee employee)
        {
            if (employee.Age < 18)
                throw new UnderAgeClientException("Сотрудник моложе 18 лет");
            
            if (string.IsNullOrEmpty(employee.PasNumber))
                throw new MissingPassportException("Сотрудник не имеет паспортных данных");

            _employeeStorage.AddEmployee(employee);
        }

        public void EditEmployee(Employee employee, Employee updatedEmployee)
        {
            if (employee == null)
                throw new Exception("Сотрудник не найден");
            
            _employeeStorage.UpdateEmployee(employee, updatedEmployee);
            
        }

        public List<Employee> GetFilterEmployees(string fullName, string phoneNumber, string pasNumber, int? minAge,
            int? maxAge)
        {
            return _employeeStorage.GetFilterEmployees(fullName, phoneNumber, pasNumber, minAge, maxAge);
        }

        public Employee GetYoungestEmployee()
        {
            return _employeeStorage.GetYoungestEmployee();
        }

        public Employee GetOldestEmployee()
        {
            return _employeeStorage.GetOldestEmployee();
        }

        public double GetAverageAge()
        {
            return _employeeStorage.GetAverageAge();
        }
}