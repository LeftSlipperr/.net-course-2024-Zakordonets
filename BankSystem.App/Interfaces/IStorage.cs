using System.Collections.ObjectModel;
using BankSystem.Models;
using Microsoft.VisualBasic;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, TResult>
{
    TResult Get(Func<T, bool> filter);
    void Add(T item);
    void Update(T item);
    void Delete(T item);
}