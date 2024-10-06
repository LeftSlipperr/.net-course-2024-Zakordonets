using BankSystem.Models;

namespace ClientStorage;

public class ClientStorage
{
    private Dictionary<Client, List<Account>> _clients = new Dictionary<Client, List<Account>>();

    public void AddClient(Client client, List<Account> accounts)
    {
        _clients.Add(client, accounts);
    }

    public void AddNewAccount(Client client, Account account)
    {
        _clients[client].Add(account);
    }

    public void UpdateAccount(Client client, Account account)
    {
        Account existingAccount = _clients[client].FirstOrDefault(a => a.Currency.Equals(account.Currency));

        if (existingAccount != null)
        {
            existingAccount.Amount = account.Amount;
            existingAccount.Currency = account.Currency;
        }
        else
        {
            throw new Exception("Счет не найден");
        }
    }

    public List<Client> GetFilteredClients(string fullName, string phoneNumber, string passportNumber, int? minAge, int? maxAge)
    {
        return _clients.Keys.Where(c => 
            (string.IsNullOrWhiteSpace(fullName) || c.FullName.Contains(fullName)) &&
            (string.IsNullOrWhiteSpace(phoneNumber) || c.PhoneNumber.Contains(phoneNumber)) &&
            (string.IsNullOrWhiteSpace(passportNumber) || c.PasNumber.Contains(passportNumber)) &&
            (!minAge.HasValue || c.Age >= minAge.Value) &&
            (!maxAge.HasValue || c.Age <= maxAge.Value)
        ).ToList();
    }

    public Client GetYoungestClient()
    {
        return _clients.OrderBy(c => c.Key.Age).FirstOrDefault().Key;
    }

    public Client GetOldestClient()
    {
        return _clients.OrderByDescending(c => c.Key.Age).FirstOrDefault().Key;
    }

    public double GetAverageAge()
    {
        return _clients.Average(c => c.Key.Age);
    }

    public Dictionary<Client, List<Account>> GetAllClients()
    {
        return new Dictionary<Client, List<Account>>(_clients);
    }

}