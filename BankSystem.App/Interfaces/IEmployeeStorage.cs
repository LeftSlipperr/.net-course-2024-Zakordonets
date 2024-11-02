using BankSystem.Models;

namespace BankSystem.App.Interfaces;

public interface IEmployeeStorage : IStorage<Employee,List<Employee>>
{
    public Task<List<Employee>> GetEmployeesByParameters(
        string name = null, string secondName = null, string thirdName = null,
        string phoneNumber = null, string pasNumber = null,
        int? age = null, string contract = null, decimal? salary = null,
        bool? isOwner = null, int pageNumber = 1, int pageSize = 10, string sortBy = "Name");
}