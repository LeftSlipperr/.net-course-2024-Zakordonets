using System.Collections;
using System.Collections.ObjectModel;
using BankSystem.Models;
using Microsoft.VisualBasic;

namespace BankSystem.App.Interfaces;

public interface IStorage<T, TResult>
{
    TResult Get(Guid id);
    void Add(T item);
    void Update(T item);
    void Delete(Guid id);
}