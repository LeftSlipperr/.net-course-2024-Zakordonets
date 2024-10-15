using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Services
{
    public class EmployeeService 
    {
        private readonly IStorage<Employee, List<Employee>> _employeeStorage;
    
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
                throw new ArgumentNullException(nameof(employee), "Сотрудник не найден");
            
            var existingEmployee = _employeeStorage.Get(employee.Id).FirstOrDefault();
            if (existingEmployee == null)
                throw new KeyNotFoundException("Сотрудник не найден");

            _employeeStorage.Update(employee.Id, employee);
        }

        public List<Employee> Get(Employee employee)
        {
            return _employeeStorage.Get(employee.Id);
        }

        public void DeleteEmployee(Employee employee)
        {
            _employeeStorage.Delete(employee.Id);
        }
    }
}
