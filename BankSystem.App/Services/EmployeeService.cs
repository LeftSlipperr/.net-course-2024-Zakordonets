using AutoMapper;
using BankSystem.App.DTO;
using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeStorage _employeeStorage;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeStorage employeeStorage, IMapper mapper)
        {
            _employeeStorage = employeeStorage;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid userId)
        {
            var employee = await _employeeStorage.GetUserAsync(userId);
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }

        public async Task AddEmployeeAsync(EmployeeDto employeeDto)
        {
            if (employeeDto.Age < 18)
                throw new UnderAgeClientException("Сотрудник моложе 18 лет");
            
            if (string.IsNullOrEmpty(employeeDto.PasNumber))
                throw new MissingPassportException("Сотрудник не имеет паспортных данных");

            var employee = _mapper.Map<Employee>(employeeDto);
            await _employeeStorage.AddAsync(employee);
        }

        public Task UpdateEmployeeAsync(Guid id, EmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            return _employeeStorage.UpdateAsync(id, employee);
        }

        public async Task<List<Employee>> GetAsync(Employee employee)
        {
            return await _employeeStorage.GetAsync(employee.Id);
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            await _employeeStorage.DeleteAsync(id);
        }

        public async Task<EmployeeDto> FindEmployeeAsync(string? name, string? secondName, string? thirdName,
            string? phoneNumber, string? pasNumber,
            int? age)
        {
            int pageNumber = 1;
            int pageSize = 10;
            string sortBy = "Name";
        
            var employees = await _employeeStorage.GetEmployeesByParameters(name, secondName, thirdName, phoneNumber, pasNumber, age);
        
            var employeeDto = _mapper.Map<EmployeeDto>(employees.FirstOrDefault());
            return employeeDto;
        }
    }
}
