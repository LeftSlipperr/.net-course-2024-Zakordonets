namespace BankSystem.Models;

public class Employee : Person
{
   public bool IsOwner { get; set; }
   public int Sallary { get; set; }
   public string Contract { get; set; } //TODO: Создать метод и присвоить сюда что либо
   
   public int Number { get; set; }
   public string PasNumber { get; set; }
   
}