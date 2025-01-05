using BankSystem.App.Interfaces;

namespace BankSystem.App.Services;

public class RateUpdaterService
{
    
    private IClientStorage _clientStorage; 
    
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
                        account.Amount += 100;
                        await _clientStorage.UpdateAccountAsync(account);
                    
                }
            }
            
            await Task.Delay(1000, cancellationToken);
        }
    }
}