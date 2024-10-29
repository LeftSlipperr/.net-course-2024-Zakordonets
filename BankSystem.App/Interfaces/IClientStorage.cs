using BankSystem.Models;

namespace BankSystem.App.Interfaces;        

public interface IClientStorage : IStorage<Client, Dictionary<Client, List<Account>>>
{
    Task AddAccountAsync(Client client, Account account);
    Task UpdateAccountAsync(Account account);
    Task DeleteAccountAsync(Guid id);
    Task<Dictionary<Client, List<Account>>> GetAllClientsWithAccountsAsync();
}