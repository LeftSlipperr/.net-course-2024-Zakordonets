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

        public async Task AddEmployeeAsync(Employee employee)
        {
            if (employee.Age < 18)
                throw new UnderAgeClientException("Сотрудник моложе 18 лет");
            
            if (string.IsNullOrEmpty(employee.PasNumber))
                throw new MissingPassportException("Сотрудник не имеет паспортных данных");

            await _employeeStorage.AddAsync(employee);
        }

        public async Task EditEmployeeAsync(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "Сотрудник не найден");
            
            var existingEmployee =  await _employeeStorage.GetAsync(employee.Id);
            if (existingEmployee.FirstOrDefault() == null)
                throw new KeyNotFoundException("Сотрудник не найден");

            await _employeeStorage.UpdateAsync(employee);
        }

        public async Task<List<Employee>> GetAsync(Employee employee)
        {
            return await _employeeStorage.GetAsync(employee.Id);
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            await _employeeStorage.DeleteAsync(employee.Id);
        }
    }
}
