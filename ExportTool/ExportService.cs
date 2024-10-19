using System.Globalization;
using BankSystem.Models;
using ClientStorage;
using CsvHelper;
using Microsoft.EntityFrameworkCore;

namespace ExportTool;

public class ExportService
{
    private BankSystem.Infrastructure.ClientStorage _clientStorage;

    public ExportService()
    {
        _clientStorage = new BankSystem.Infrastructure.ClientStorage(new BankSystemDbContext());
    }
    
    public void WriteClientsToCsv()
    {
        string fullPath = Path.Combine("C:", "Users", "Admin", "Desktop", "test.csv");

        var clients = _clientStorage.GetListOfclients();
      
        using (var writer = new StreamWriter(fullPath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(clients);
        }
    }
    
    public List<Client> ReadClientsFromCsv()
    {
        string fullPath = Path.Combine("C:", "Users", "Admin", "Desktop", "test.csv");
      
        using (var reader = new StreamReader(fullPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var clients = csv.GetRecords<Client>().ToList();
            return clients;
        }
    }
}