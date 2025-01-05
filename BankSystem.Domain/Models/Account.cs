namespace BankSystem.Models;

public class Account
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string CurrencyName { get; set; }
    public decimal Amount { get; set; }
}