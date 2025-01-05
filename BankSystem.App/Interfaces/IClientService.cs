using BankSystem.App.DTO;

namespace BankSystem.App.Interfaces;

public interface IClientService
{
    Task<ClientDto> GetClientAsync(Guid userId);
    Task<Guid> AddClientAsync(ClientDto clientDto);
    Task UpdateClientAsync(Guid id, ClientDto clientDto);
    Task DeleteClientAsync(Guid id);
    Task<ClientDto> FindClientAsync(string? name, string? secondName, string? thirdName,
        string? phoneNumber, string? pasNumber,
        int? age, int? accountNumber, decimal? balance);
}