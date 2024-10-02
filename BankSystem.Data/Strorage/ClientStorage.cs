using BankSystem.Models;

namespace ClientStorage;

public class ClientStorage
{
    private List<Client> _clients = new List<Client>();

    public void AddClient(Client client)
    {
        _clients.Add(client);
    }

    public Client GetYoungestClient()
    {
        return _clients.OrderBy(c => c.Age).FirstOrDefault();
    }

    public Client GetOldestClient()
    {
        return _clients.OrderByDescending(c => c.Age).FirstOrDefault();
    }

    public double GetAverageAge()
    {
        return _clients.Average(c => c.Age);
    }
}