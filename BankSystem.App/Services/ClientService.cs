using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Services;

public class ClientService
{
    private ClientStorage.ClientStorage _clientStorage;
    private Dictionary<Client, List<Account>> _clientsAccount;
        
    
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
    
    public void AddAccountToClient(string passportNumber, Account account)
    {
        var client = _clientStorage.GetAllClients()
            .FirstOrDefault(c => c.Key.PasNumber == passportNumber);
        
        if (client.Key == null)
            throw new Exception("Клиента не существует");

        if (client.Key.PasNumber == "")
            throw new MissingPassportException("Клиент с таким паспортом не найден");

        if (client.Value.Any(a => a.Currency.Equals(account.Currency)))
            throw new Exception("У клиента уже есть счёт в этой валюте");

        _clientStorage.AddNewAccount(client.Key, account);
    }
    
    public void EditAccount(string passportNumber, Account oldAccount, Account newAccount)
    {
        var client = _clientStorage.GetAllClients()
            .FirstOrDefault(c => c.Key.PasNumber == passportNumber);

        if (client.Key == null)
            throw new MissingPassportException("Клиент с таким паспортом не найден");

        Account accountToEdit = client.Value.FirstOrDefault(a => a.Currency.Equals(oldAccount.Currency));

        if (accountToEdit == null)
            throw new Exception("Счёт не найден");
        
        _clientStorage.UpdateAccount(client.Key, newAccount);
    }
    
    public List<Client> GetFilteredClients(string fullName, string phoneNumber, string passportNumber, int? minAge, int? maxAge)
    {
        return _clientStorage.GetAllClients().Keys
            .Where(c => 
                (string.IsNullOrWhiteSpace(fullName) || c.FullName.Contains(fullName)) &&
                (string.IsNullOrWhiteSpace(phoneNumber) || c.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrWhiteSpace(passportNumber) || c.PasNumber.Contains(passportNumber)) &&
                (!minAge.HasValue || c.Age >= minAge.Value) &&
                (!maxAge.HasValue || c.Age <= maxAge.Value)
            ).ToList();
    }
}