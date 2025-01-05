namespace BankSystem.Models;

public class Employee : Person
{
   public bool IsOwner { get; set; }
   public int Salary { get; set; }
   public string Contract { get; set; }
   public string PhoneNumber { get; set; }
   public string PasNumber { get; set; }
   
   public override bool Equals(object obj)
   {
      if (obj is Employee otherEmployee)
      {
         return this.PasNumber == otherEmployee.PasNumber;
      }
      return false;
   }

   public override int GetHashCode()
   {
      return this.PasNumber.GetHashCode();
   }
}