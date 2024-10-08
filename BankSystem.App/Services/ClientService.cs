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
        if (client == null)
            throw new Exception("Клиента не существует");

        if (client.PasNumber == "")
            throw new MissingPassportException("Клиент с таким паспортом не найден");

        _clientStorage.AddAccount(client, account);
    }
    
    public void UpdateClient(Client client)
    {

        if (client == null)
            throw new MissingPassportException("Клиент с таким паспортом не найден");
        
        _clientStorage.Update(client);
    }

    public void DeleteClient(Client client)
    {
        _clientStorage.Delete(client);
    }

    public void DeleteAccount(Client client, Account accountToDelete)
    {
        _clientStorage.DeleteAccount(client, accountToDelete);
    }
    
    public void UpdateAccount(Client client, Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
        }

        _clientStorage.UpdateAccount(client, account); 
    }
    
}