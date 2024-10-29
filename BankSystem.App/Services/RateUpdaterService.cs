using BankSystem.App.Interfaces;

namespace BankSystem.App.Services;

public class RateUpdaterService
{
    
    private IClientStorage _clientStorage; 
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public RateUpdaterService(IClientStorage clientStorage)
    {
         _clientStorage = clientStorage;
    }

    public async Task UpdateRate(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var clientsWithAccounts = await _clientStorage.GetAllClientsWithAccountsAsync();

            foreach (var clientAccounts in clientsWithAccounts.Values)
            {
                foreach (var account in clientAccounts)
                {
                    await _semaphore.WaitAsync(cancellationToken);
                    try
                    {
                        account.Amount += 100;
                        await _clientStorage.UpdateAccountAsync(account);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            }
            
            await Task.Delay(1000, cancellationToken);
        }
    }
}