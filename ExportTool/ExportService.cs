using System.Globalization;
using BankSystem.Models;
using ClientStorage;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExportTool;

public class ExportService
{
    private const int MaxFileSize = 100 * 100;
    private int _fileIndex = 1;

    private readonly BankSystem.Data.Storage.ClientStorage _clientStorage;

    public ExportService(BankSystem.Data.Storage.ClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
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