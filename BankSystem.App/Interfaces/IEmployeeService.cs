using BankSystem.App.DTO;

namespace BankSystem.App.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto> GetEmployeeAsync(Guid userId);
    Task AddEmployeeAsync(EmployeeDto employeeDto);
    Task UpdateEmployeeAsync(Guid id, EmployeeDto employeeDto);
    Task DeleteEmployeeAsync(Guid id);
    Task<EmployeeDto> FindEmployeeAsync(string? name, string? secondName, string? thirdName,
        string? phoneNumber, string? pasNumber,
        int? age);
}