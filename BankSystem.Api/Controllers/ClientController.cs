using BankSystem.App.DTO;
using BankSystem.App.Interfaces;
using BankSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("GetClient")]
    public async Task<IActionResult> GetClient([FromRoute] Guid guid) 
    {
        var response = await _clientService.GetClientAsync(guid);

        return Ok(response);
    }

    [HttpPost]
    public async Task AddClient([FromBody] ClientDto client)
    {
        if (client == null)
        {
            BadRequest("Client cannot be null.");
        }
        
        await _clientService.AddClientAsync(client);
        
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientDto clientDto)
    {
        await _clientService.UpdateClientAsync(id, clientDto);
        return Ok();
    }

    [HttpDelete]
    public async Task DeleteClient([FromQuery] Guid guid)
    {
        await _clientService.DeleteClientAsync(guid);
    }

    [HttpGet("FindClient")]
    public async Task<IActionResult> FindClient(string? name, string? secondName, string? thirdName,
        string? phoneNumber, string? pasNumber,
        int? age, int? accountNumber, decimal? balance)
    {
        var response = await _clientService.FindClientAsync(name, secondName, thirdName, phoneNumber, pasNumber, age, accountNumber, balance);
        return Ok(response);
    }
}