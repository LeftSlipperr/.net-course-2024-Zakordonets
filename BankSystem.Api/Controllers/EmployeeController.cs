using BankSystem.App.DTO;
using BankSystem.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("GetEmployee")]
    public async Task<IActionResult> GetEmployee([FromQuery] Guid guid) 
    {
        var response = await _employeeService.GetEmployeeAsync(guid);

        return Ok(response);
    }

    [HttpPost]
    public async Task AddEmployee([FromQuery] EmployeeDto employeeDto)
    {
        if (employeeDto == null)
        {
            BadRequest("Client cannot be null.");
        }
        
        await _employeeService.AddEmployeeAsync(employeeDto);
        
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromQuery] EmployeeDto employeeDto)
    {
        await _employeeService.UpdateEmployeeAsync(id, employeeDto);
        return Ok();
    }

    [HttpDelete]
    public async Task DeleteEmployee([FromQuery] Guid guid)
    {
        await _employeeService.DeleteEmployeeAsync(guid);
    }

    [HttpGet("FindClient")]
    public async Task<IActionResult> FindClient(string? name, string? secondName, string? thirdName,
        string? phoneNumber, string? pasNumber,
        int? age)
    {
        var response = await _employeeService.FindEmployeeAsync(name, secondName, thirdName, phoneNumber, pasNumber, age);
        return Ok(response);
    }
}