using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Services;

public class EmployeeService 
{
        private IStorage<Employee, List<Employee>> _employeeStorage;
    
        public EmployeeService(IStorage<Employee, List<Employee>> employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public void AddEmployee(Employee employee)
        {
            if (employee.Age < 18)
                throw new UnderAgeClientException("Сотрудник моложе 18 лет");
            
            if (string.IsNullOrEmpty(employee.PasNumber))
                throw new MissingPassportException("Сотрудник не имеет паспортных данных");

            _employeeStorage.Add(employee);
        }

        public void EditEmployee(Employee employee)
        {
            if (employee == null)
                throw new Exception("Сотрудник не найден");
            
            _employeeStorage.Update(employee);
            
        }

        public List<Employee> GetFilterEmployees(string fullName, string phoneNumber, string pasNumber, int? minAge, int? maxAge)
        {
            return _employeeStorage.Get(e =>
                (string.IsNullOrWhiteSpace(fullName) || e.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(phoneNumber) || e.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrWhiteSpace(pasNumber) || e.PasNumber.Contains(pasNumber, StringComparison.OrdinalIgnoreCase)));
        }

        public void DeleteEmployee(Employee employee)
        {
            _employeeStorage.Delete(employee);
        }

}