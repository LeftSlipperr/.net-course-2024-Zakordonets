using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Services;

public class ClientService
{
    private IClientStorage _clientStorage; 
    private static readonly SemaphoreSlim _dbSemaphore = new SemaphoreSlim(1, 1);

    public ClientService(IClientStorage clientStorage)
    {
        _clientStorage = clientStorage; 
    }

    public async Task<Dictionary<Client, List<Account>>> GetAsync(Client client)
    {
        return await _clientStorage.GetAsync(client.Id);
    }
    
    public async Task AddAsync(Client client)
    {
        if (client.Age < 18)
            throw new UnderAgeClientException("Клиент моложе 18 лет");

        if (client.PasNumber == "")
            throw new MissingPassportException("Клиент не имеет паспортных данных");
        
        await _clientStorage.AddAsync(client);    
    }
    
    public async Task AddAccountToClientAsync(Client client, Account account)
    {
        await _clientStorage.AddAccountAsync(client, account);
    }
    
    public async Task UpdateClientAsync(Client client)
    {

        if (client == null)
            throw new MissingPassportException("Клиент с таким паспортом не найден");
        
        await _clientStorage.UpdateAsync(client);
    }

    public async Task DeleteClientAsync(Client client)
    {
        await _clientStorage.DeleteAsync(client.Id);
    }

    public async Task DeleteAccountAsync(Account accountToDelete)
    {
        await _clientStorage.DeleteAccountAsync(accountToDelete.Id);
    }
    
    public async Task UpdateAccountAsync(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
        }

        await _clientStorage.UpdateAccountAsync(account); 
    }

    public async Task WithdrawFromAccountAsync(Client client, decimal amountToWithdraw)
    {
        await _dbSemaphore.WaitAsync();
        try
        {
            var clientAccountsDictionary = await _clientStorage.GetAsync(client.Id);

            if (clientAccountsDictionary.TryGetValue(client, out var clientAccounts))
            {
                foreach (var account in clientAccounts)
                {
                    if (account.Amount >= amountToWithdraw)
                    {
                        account.Amount -= amountToWithdraw;
                        await _clientStorage.UpdateAccountAsync(account);
                        return;
                    }
                }

                throw new Exception($"Недостаточно средств на счетах клиента {client.Name} для списания {amountToWithdraw}.");
            }

            throw new Exception($"Клиент с ID {client.Id} не найден.");
        }
        finally
        {
            _dbSemaphore.Release();
        }
    }



}