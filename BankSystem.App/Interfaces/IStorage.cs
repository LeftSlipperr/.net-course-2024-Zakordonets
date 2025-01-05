using System.Collections;
using System.Collections.ObjectModel;
using BankSystem.Models;
using Microsoft.VisualBasic;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, TResult>
{
    Task<TResult> GetAsync(Guid id);
    Task<T> GetUserAsync (Guid id);
    Task<Guid> AddAsync(T item);
    Task UpdateAsync(Guid id, T item);
    Task DeleteAsync(Guid id);
}