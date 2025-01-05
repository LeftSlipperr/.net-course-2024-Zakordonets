namespace BankSystem.Models;

public struct Currency
{
    public string Symbol;
    public string CurrencyName;
    
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