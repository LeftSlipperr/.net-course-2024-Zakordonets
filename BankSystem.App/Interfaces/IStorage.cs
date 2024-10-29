using System.Collections;
using System.Collections.ObjectModel;
using BankSystem.Models;
using Microsoft.VisualBasic;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, TResult>
{
    Task<TResult> GetAsync(Guid id);
    Task AddAsync(T item);
    Task UpdateAsync(T item);
    Task DeleteAsync(Guid id);
}