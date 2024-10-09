using BankSystem.App.Interfaces;
using BankSystem.Models;

namespace BankSystem.Infrastructure;

public class ClientStorage : IClientStorage
{
    private Dictionary<Client, List<Account>> _clientAccounts;

    public ClientStorage()
    {
        _clientAccounts = new Dictionary<Client, List<Account>>();
    }
    
    public Dictionary<Client, List<Account>> Get(Func<Client, bool> filter)
    {
        return _clientAccounts
            .Where(kvp => filter(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
        
    public void Add(Client client)
    {
        Account defaultAccount = (new Account(){
            Amount = 0,
            Currency = new Currency
            {
                CurrencyName = "USD",
                Symbol = "$"
            }
        });

        _clientAccounts[client] = new List<Account> { defaultAccount };
    }
    
    public void Update(Client client)
    {
        Client newClient = _clientAccounts.Keys
            .FirstOrDefault(c => c.PasNumber == client.PasNumber);

        client.FullName = newClient.FullName;
        client.Age = newClient.Age;
        client.PasNumber = client.PasNumber;
        client.PhoneNumber = client.PhoneNumber;
    }

    public void Delete(Client client)
    {
        _clientAccounts.Remove(client);
    }

    public void AddAccount(Client client, Account newAccount)
    {
        _clientAccounts[client].Add(newAccount);
    }

    public void UpdateAccount(Client client, Account newAccount)
    {
        _clientAccounts[client].Add(newAccount);
    }

    public void DeleteAccount(Client client, Account accountToDelete)
    {
        _clientAccounts[client].Remove(accountToDelete);
    }
}