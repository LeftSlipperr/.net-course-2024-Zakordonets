namespace BankSystem.Models;

public class Client : Person
{
    public string PhoneNumber { get; set; }
    public string PasNumber { get; set; }

   public override bool Equals(object obj)
    {
        if (obj is Client otherClient)
        {
            return this.PasNumber == otherClient.PasNumber;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return this.PasNumber.GetHashCode();
    }
}