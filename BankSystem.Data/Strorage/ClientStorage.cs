using BankSystem.Models;

namespace ClientStorage;

public class ClientStorage
{
    private List<Client> clients = new List<Client>();

    public void AddClient(Client client)
    {
        clients.Add(client);
    }

    public Client GetYoungestClient()
    {
        return clients.OrderBy(c => c.Age).FirstOrDefault();
    }

    public Client GetOldestClient()
    {
        return clients.OrderByDescending(c => c.Age).FirstOrDefault();
    }

    public double GetAverageAge()
    {
        return clients.Average(c => c.Age);
    }
    public List<Client> GetAllClients()
    {
        return clients;
    }
}