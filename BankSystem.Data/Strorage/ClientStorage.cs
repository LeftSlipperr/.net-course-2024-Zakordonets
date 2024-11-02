using BankSystem.App.Interfaces;
using BankSystem.Models;
using ClientStorage;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data.Storage
{
    public class ClientStorage : IClientStorage
    {
        private readonly BankSystemDbContext _dbContext;

        public ClientStorage(BankSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dictionary<Client, List<Account>>> GetAsync(Guid id)
        {
                var clientWithAccounts = await _dbContext.Clients
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

        public async Task<Client> GetUserAsync(Guid id)
        {
            var client = await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.Id == id);
            return client;
        }

        public async Task AddAsync(Client client)
        {
            if (client.Id == Guid.Empty)
            {
                client.Id = Guid.NewGuid();
            }

            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();

            var defaultAccount = new Account
            {
                ClientId = client.Id,
                Id = Guid.NewGuid(),
                Amount = 0,
                CurrencyName = "USD"
            };

            await _dbContext.Accounts.AddAsync(defaultAccount);
            await _dbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Guid id, Client client)
        {
            client.Id = id;
            var existingClient = await _dbContext.Clients
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

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var clientToRemove = await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clientToRemove != null)
            {
                _dbContext.Clients.Remove(clientToRemove);
                await _dbContext.SaveChangesAsync(); 
            }
        }

        public async Task AddAccountAsync(Client client, Account account)
        {
            account.ClientId = client.Id;
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            
                var existingAccount = await _dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.Id == account.Id);

                if (existingAccount != null)
                {
                    existingAccount.CurrencyName = account.CurrencyName;
                    existingAccount.Amount = account.Amount;

                    await _dbContext.SaveChangesAsync();
                }
            
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var accountToRemove = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Id == id);

            if (accountToRemove != null)
            {
                _dbContext.Accounts.Remove(accountToRemove);
                await _dbContext.SaveChangesAsync();
            }
        }
        
        public async Task<List<Client>> GetClientsByParametersAsync(
            string? name = null, string? secondName = null, string? thirdName = null, 
            string? phoneNumber = null, string? pasNumber = null, 
            int? age = null, int? accountNumber = null, decimal? balance = null,
            int pageNumber = 1, int pageSize = 10, string sortBy = "Name")
        {
            var query = _dbContext.Clients.AsQueryable();
            
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
            return await _dbContext.Clients
                .Include(c => c.Accounts)  // Загрузка связанных аккаунтов
                .ToDictionaryAsync(
                    client => client, 
                    client => client.Accounts.ToList()
                );
        }


    }
}
