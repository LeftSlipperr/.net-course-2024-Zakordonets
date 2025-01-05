using BankSystem.Models;

namespace BankSystem.App.Interfaces;        

public interface IClientStorage : IStorage<Client, Dictionary<Client, List<Account>>>
{
    Task AddAccountAsync(Client client, Account account);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(Guid id);
    Task<Dictionary<Client, List<Account>>> GetAllClientsWithAccountsAsync();

    Task<List<Client>> GetClientsByParametersAsync(
        string? name = null, string? secondName = null, string? thirdName = null,
        string? phoneNumber = null, string? pasNumber = null,
        int? age = null, int? accountNumber = null, decimal? balance = null,
        int pageNumber = 1, int pageSize = 10, string sortBy = "Name");
}