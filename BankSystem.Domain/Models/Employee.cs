namespace BankSystem.Models;

public class Employee : Person
{
   public bool IsOwner { get; set; }
   public int Sallary { get; set; }
   public string Contract { get; set; }
   
   public int Number { get; set; }
   public string PasNumber { get; set; }
   
}