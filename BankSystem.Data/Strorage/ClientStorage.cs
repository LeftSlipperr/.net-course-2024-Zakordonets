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

        public async Task<Dictionary<Client, List<Account>>> GetAsync(Guid id)
        {
                var clientWithAccounts = await _bankSystemDbContext.Clients
                    .Include(c => c.Accounts)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (clientWithAccounts != null)
                {
                    return new Dictionary<Client, List<Account>>
                    {
                        { clientWithAccounts, clientWithAccounts.Accounts.ToList() }
                    };
                }

                return new Dictionary<Client, List<Account>>();
        }

        public async Task AddAsync(Client client)
        {
            if (client.Id == Guid.Empty)
            {
                client.Id = Guid.NewGuid();
            }

            await _bankSystemDbContext.Clients.AddAsync(client);
            await _bankSystemDbContext.SaveChangesAsync();

            var defaultAccount = new Account
            {
                ClientId = client.Id,
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "USD"
            };

            await _bankSystemDbContext.Accounts.AddAsync(defaultAccount);
            await _bankSystemDbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Client client)
        {
            var existingClient = await _bankSystemDbContext.Clients
                .FirstOrDefaultAsync(c => c.Id == client.Id);

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

                await _bankSystemDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var clientToRemove = await _bankSystemDbContext.Clients
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clientToRemove != null)
            {
                _bankSystemDbContext.Clients.Remove(clientToRemove);
                await _bankSystemDbContext.SaveChangesAsync(); 
            }
        }

        public async Task AddAccountAsync(Client client, Account account)
        {
            account.ClientId = client.Id;
            await _bankSystemDbContext.Accounts.AddAsync(account);
            await _bankSystemDbContext.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            
                var existingAccount = await _bankSystemDbContext.Accounts
                    .FirstOrDefaultAsync(a => a.Id == account.Id);

                if (existingAccount != null)
                {
                    existingAccount.CurrencyName = account.CurrencyName;
                    existingAccount.Amount = account.Amount;

                    await _bankSystemDbContext.SaveChangesAsync();
                }
            
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var accountToRemove = await _bankSystemDbContext.Accounts
                .FirstOrDefaultAsync(a => a.Id == id);

            if (accountToRemove != null)
            {
                _bankSystemDbContext.Accounts.Remove(accountToRemove);
                await _bankSystemDbContext.SaveChangesAsync();
            }
        }
        
        public async Task<List<Client>> GetClientsByParametersAsync(
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
            
            return await query.ToListAsync();
        }
        
        public async Task<Dictionary<Client, List<Account>>> GetAllClientsWithAccountsAsync()
        {
            return await _bankSystemDbContext.Clients
                .Include(c => c.Accounts)  // Загрузка связанных аккаунтов
                .ToDictionaryAsync(
                    client => client, 
                    client => client.Accounts.ToList()
                );
        }


    }
}
