using AutoMapper;
using BankSystem.App.DTO;
using BankSystem.App.Interfaces;
using BankSystem.App.Services.Exceptions;
using BankSystem.Models;

namespace BankSystem.App.Services;

public class ClientService : IClientService
{ 
    private IClientStorage _storage;
    private readonly IMapper _mapper;

    public ClientService(IClientStorage storage, IMapper mapper) 
    {
        _storage = storage;    
        _mapper = mapper;
    }

    public async Task<Dictionary<Client, List<Account>>> GetAsync(Guid id)
    {
        return  await _storage.GetAsync(id);
    }

    public async Task<ClientDto> GetClientAsync(Guid id)
    {
        var client = await _storage.GetUserAsync(id);
        var clientDto = _mapper.Map<ClientDto>(client);

        return clientDto;
    }

    public async Task AddClientAsync(ClientDto clientDto)
    {
        if (clientDto.Age < 18)
            throw new UnderAgeClientException("Клиент моложе 18 лет");

        if (clientDto.PasNumber == "")
            throw new MissingPassportException("Клиент не имеет паспортных данных");
        
        var client = _mapper.Map<Client>(clientDto); 
        await _storage.AddAsync(client);
    }
    
    public async Task AddAccountAsync(Client client, Account account)
    {
        await _storage.AddAccountAsync(client, account);
    }
    
    public async Task UpdateClientAsync(Guid id, ClientDto clientDto)
    {

        if (clientDto == null)
            throw new MissingPassportException("Клиент с таким паспортом не найден");
        
        var client = _mapper.Map<Client>(clientDto); 
        await _storage.UpdateAsync(id, client);
    }

    public async Task DeleteClientAsync(Guid id)
    {
        await _storage.DeleteAsync(id);
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        await _storage.DeleteAccountAsync(id);
    }
    
    public async Task UpdateAccountAsync(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Лицевой счет не может быть нулевым."); 
        }

        await _storage.UpdateAccountAsync(account); 
    }

    public async Task<Dictionary<Client, List<Account>>> GetAllClientsWithAccountsAsync()
    {
        return await _storage.GetAllClientsWithAccountsAsync();
    }

    public async Task<ClientDto> FindClientAsync(string? name, string? secondName, string? thirdName,
        string? phoneNumber, string? pasNumber,
        int? age, int? accountNumber, decimal? balance)
    {
        int pageNumber = 1;
        int pageSize = 10;
        string sortBy = "Name";
        
        var clients = await _storage.GetClientsByParametersAsync(name, secondName, thirdName, phoneNumber, pasNumber, age, accountNumber, balance, pageNumber, pageSize, sortBy);
        
        var clientsDto = _mapper.Map<ClientDto>(clients.FirstOrDefault());
        return clientsDto;
    }

    public async Task WithdrawFromAccountAsync(Client client, decimal amountToWithdraw)
    {
            var clientAccountsDictionary = await _storage.GetAsync(client.Id);

            if (clientAccountsDictionary.TryGetValue(client, out var clientAccounts))
            {
                foreach (var account in clientAccounts)
                {
                    if (account.Amount >= amountToWithdraw)
                    {
                        account.Amount -= amountToWithdraw;
                        await _storage.UpdateAccountAsync(account);
                        return;
                    }
                }

                throw new Exception($"Недостаточно средств на счетах клиента {client.Name} для списания {amountToWithdraw}.");
            }

            throw new Exception($"Клиент с ID {client.Id} не найден.");
        
    }



}