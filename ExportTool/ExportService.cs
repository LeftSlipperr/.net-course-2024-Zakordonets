using System.Globalization;
using BankSystem.Models;
using ClientStorage;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExportTool;

public class ExportService
{
    private BankSystem.Infrastructure.ClientStorage _clientStorage;

    public ExportService()
    {
        _clientStorage = new BankSystem.Infrastructure.ClientStorage(new BankSystemDbContext());
    }
    
    public void WriteClientsToCsv(List<Client> listOfClients, string fullPath)
    {
        var clients = listOfClients;
      
        using (var writer = new StreamWriter(fullPath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(clients);
        }
    }
    
    public async Task ReadClientsFromCsv(string fullPath)
    {
      
        using (var reader = new StreamReader(fullPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var clients = csv.GetRecords<Client>().ToList();
            foreach (var client in clients)
            {
                await _clientStorage.AddAsync(client);
            }
        }
    }
    
    public void ItemsSerialization<T>(T item, string filePath)
    {
        string json = JsonConvert.SerializeObject(item, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
    
    public T ItemsDeserialization<T>(string filePath)
    {
        string json = File.ReadAllText(filePath);
        T deserializedObject = JsonConvert.DeserializeObject<T>(json);
        return deserializedObject;
    }
}