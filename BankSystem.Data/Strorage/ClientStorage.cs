using BankSystem.App.Interfaces;
using BankSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ClientStorage;

namespace BankSystem.Infrastructure
{
    public class ClientStorage : IClientStorage
    {
        private readonly BankSystemDbContext _bankSystemDbContext;

        public ClientStorage(BankSystemDbContext bankSystemDbContext)
        {
            _bankSystemDbContext = bankSystemDbContext;
        }

        public Dictionary<Client, List<Account>> Get(Guid id)
        {
            var clientWithAccounts = _bankSystemDbContext.Clients
                .Include(c => c.Accounts) 
                .FirstOrDefault(c => c.Id == id);

            if (clientWithAccounts != null)
            {
                return new Dictionary<Client, List<Account>>
                {
                    { clientWithAccounts, clientWithAccounts.Accounts.ToList() }
                };
            }

            return new Dictionary<Client, List<Account>>();
        }

        public void Add(Client client)
        {
            if (client.Id == Guid.Empty)
            {
                client.Id = Guid.NewGuid();
            }

            _bankSystemDbContext.Clients.Add(client);
            _bankSystemDbContext.SaveChanges();

            var defaultAccount = new Account
            {
                ClientId = client.Id,
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "USD"
            };

            _bankSystemDbContext.Accounts.Add(defaultAccount);
            _bankSystemDbContext.SaveChanges();
        }


        public void Update(Client client)
        {
            var existingClient = _bankSystemDbContext.Clients
                .FirstOrDefault(c => c.Id == client.Id);

            if (existingClient != null)
            {
                existingClient.Name = client.Name;
                existingClient.SecondName = client.SecondName;
                existingClient.ThirdName = client.ThirdName;
                existingClient.Balance = client.Balance;
                existingClient.AccountNumber = client.AccountNumber;
                existingClient.Age = client.Age;
                existingClient.PasNumber = client.PasNumber;
                existingClient.PhoneNumber = client.PhoneNumber;

                _bankSystemDbContext.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var clientToRemove = _bankSystemDbContext.Clients
                .FirstOrDefault(c => c.Id == id);

            if (clientToRemove != null)
            {
                _bankSystemDbContext.Clients.Remove(clientToRemove);
                _bankSystemDbContext.SaveChanges(); 
            }
        }

        public void AddAccount(Client client, Account account)
        {
            account.ClientId = client.Id;
            _bankSystemDbContext.Accounts.Add(account);
            _bankSystemDbContext.SaveChanges();
        }

        public void UpdateAccount(Account account)
        {
            var existingAccount = _bankSystemDbContext.Accounts
                .FirstOrDefault(a => a.Id == account.Id);

            if (existingAccount != null)
            {
                existingAccount.CurrencyName = account.CurrencyName;
                existingAccount.Amount = account.Amount;

                _bankSystemDbContext.SaveChanges();
            }
        }

        public void DeleteAccount(Guid id)
        {
            var accountToRemove = _bankSystemDbContext.Accounts
                .FirstOrDefault(a => a.Id == id);

            if (accountToRemove != null)
            {
                _bankSystemDbContext.Accounts.Remove(accountToRemove);
                _bankSystemDbContext.SaveChanges();
            }
        }
        
        public List<Client> GetClientsByParameters(
            string name = null, string secondName = null, string thirdName = null, 
            string phoneNumber = null, string pasNumber = null, 
            int? age = null, int? accountNumber = null, decimal? balance = null,
            int pageNumber = 1, int pageSize = 10, string sortBy = "Name")
        {
            var query = _bankSystemDbContext.Clients.AsQueryable();
            
            if (!string.IsNullOrEmpty(name)) query = query.Where(c => c.Name.Contains(name));
            if (!string.IsNullOrEmpty(secondName)) query = query.Where(c => c.SecondName.Contains(secondName));
            if (!string.IsNullOrEmpty(thirdName)) query = query.Where(c => c.ThirdName.Contains(thirdName));
            if (!string.IsNullOrEmpty(phoneNumber)) query = query.Where(c => c.PhoneNumber.Contains(phoneNumber));
            if (!string.IsNullOrEmpty(pasNumber)) query = query.Where(c => c.PasNumber == pasNumber);
            if (age.HasValue) query = query.Where(c => c.Age == age.Value);
            if (accountNumber.HasValue) query = query.Where(c => c.AccountNumber == accountNumber.Value);
            if (balance.HasValue) query = query.Where(c => c.Balance == balance.Value);
            
            if (sortBy == "Name")
            {
                query = query.OrderBy(c => c.Name);
            }
            else if (sortBy == "Age")
            {
                query = query.OrderBy(c => c.Age);
            }
            else if (sortBy == "Balance")
            {
                query = query.OrderBy(c => c.Balance);
            }
            
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            
            return query.ToList();
        }


    }
}
