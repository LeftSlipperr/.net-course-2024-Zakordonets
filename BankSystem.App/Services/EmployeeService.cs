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

        public void EditEmployee(string passportNumber, Employee updatedEmployee)
        {
            Employee employee = _employeeStorage.GetAllEmployees()
                .FirstOrDefault(e => e.PasNumber == passportNumber);

            if (employee == null)
                throw new Exception("Сотрудник не найден");
            
            employee.FullName = updatedEmployee.FullName;
            employee.Age = updatedEmployee.Age;
            employee.Salary = updatedEmployee.Salary;
            employee.PhoneNumber = updatedEmployee.PhoneNumber;
            employee.Contract = updatedEmployee.Contract;
        }

        public List<Employee> GetFilterEmployees(string fullName, string phoneNumber, string pasNumber, int? minAge, int? maxAge)
        {
            return _employeeStorage.GetAllEmployees()
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