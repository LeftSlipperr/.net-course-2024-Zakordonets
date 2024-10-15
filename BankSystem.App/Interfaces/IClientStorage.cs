using BankSystem.Models;

namespace BankSystem.App.Interfaces;        

public interface IClientStorage : IStorage<Client, Dictionary<Client, List<Account>>>
{
    void AddAccount(Guid id, Account account);
    void UpdateAccount(Account account);
    void DeleteAccount(Guid id);
}