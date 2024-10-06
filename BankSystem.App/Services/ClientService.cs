using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Services;

public class ClientService
{
    private ClientStorage.ClientStorage _clientStorage;
    
    public ClientService(ClientStorage.ClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
    }

    public void AddClient(Client client)
    {
        if (client.Age < 18)
            throw new UnderAgeClientException("Клиент моложе 18 лет");

        if (client.PasNumber == "")
            throw new MissingPassportException("Клиент не имеет паспортных данных");

        List<Account> accounts = new List<Account>();
        accounts.Add(new Account(){
            Amount = 0,
            Currency = new Currency
            {
                CurrencyName = "USD",
                Symbol = "$"
            }
        });
        
        _clientStorage.AddClient(client, accounts);    
    }
    
    public void AddAccountToClient(Client client, Account account)
    {
        if (client == null)
            throw new Exception("Клиента не существует");

        if (client.PasNumber == "")
            throw new MissingPassportException("Клиент с таким паспортом не найден");

        _clientStorage.AddNewAccount(client, account);
    }
    
    public void EditAccount(Client client, Account newAccount)
    {

        if (client == null)
            throw new MissingPassportException("Клиент с таким паспортом не найден");
        
        _clientStorage.UpdateAccount(client, newAccount);
    }
    
    public List<Client> GetFilteredClients(string fullName, string phoneNumber, string pasportNumber, int? minAge, int? maxAge)
    {
       return  _clientStorage.GetFilteredClients(fullName, phoneNumber, pasportNumber, minAge, maxAge);
    }
}