namespace BankSystem.Models;

public class Currency
{
    public string Symbol;
    public string CurrencyName;
    public Account Account;
    public Account AccountId;
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Currency other = (Currency)obj;
        return CurrencyName == other.CurrencyName && Symbol == other.Symbol;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CurrencyName, Symbol);
    }
}