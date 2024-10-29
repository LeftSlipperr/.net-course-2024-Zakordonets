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
    private const int MaxFileSize = 100 * 100;
    private int _fileIndex = 1;

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
    
    public void ReadClientsFromCsv(string fullPath)
    {
      
        using (var reader = new StreamReader(fullPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var clients = csv.GetRecords<Client>().ToList();
            foreach (var client in clients)
            {
                _clientStorage.Add(client);
            }
        }
    }

    public void ItemsSerialization<T>(T item, string baseFilePath)
    {
        string filePath = GetFilePath(baseFilePath);
        
        string json = File.ReadAllText(filePath);
        var items = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        
        items.Add(item);
        string updatedJson = JsonConvert.SerializeObject(items, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);
    }

    public List<T> ItemsDeserialization<T>(string baseFilePath)
    {
        List<T> allItems = new List<T>();
        
        string[] files = Directory.GetFiles(baseFilePath, $"_*.json");
        
        foreach (string filePath in files)
        {
            string json = File.ReadAllText(filePath);
            
            var items = JsonConvert.DeserializeObject<List<T>>(json);
            if (items != null)
            {
                allItems.AddRange(items);
            }
        }

        return allItems;
    }


    private string GetFilePath(string baseFilePath)
    {
        string filePath = Path.Combine(baseFilePath, $"_{_fileIndex}.json");
        
        while (File.Exists(filePath) && new FileInfo(filePath).Length >= MaxFileSize)
        {
            _fileIndex++;
            filePath = Path.Combine(baseFilePath, $"_{_fileIndex}.json");
        }
        
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }

        return filePath;
    }

}