using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Services;

public class ClientService
{
    private IClientStorage _clientStorage; 

    public ClientService(IClientStorage clientStorage)
    {
        _clientStorage = clientStorage; 
    }

    public Dictionary<Client, List<Account>> Get(Client client)
    {
        return _clientStorage.Get(client.Id);
    }
    
    public void Add(Client client)
    {
        if (client.Age < 18)
            throw new UnderAgeClientException("Клиент моложе 18 лет");

        if (client.PasNumber == "")
            throw new MissingPassportException("Клиент не имеет паспортных данных");
        
        _clientStorage.Add(client);    
    }
    
    public void AddAccountToClient(Client client, Account account)
    {
        _clientStorage.AddAccount(client.Id, account);
    }
    
    public void UpdateClient(Client client)
    {

        if (client == null)
            throw new MissingPassportException("Клиент с таким паспортом не найден");
        
        _clientStorage.Update(client.Id, client);
    }

    public void DeleteClient(Client client)
    {
        _clientStorage.Delete(client.Id);
    }

    public void DeleteAccount(Account accountToDelete)
    {
        _clientStorage.DeleteAccount(accountToDelete.Id);
    }
    
    public void UpdateAccount(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
        }

        _clientStorage.UpdateAccount(account); 
    }


}