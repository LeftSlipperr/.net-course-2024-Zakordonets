namespace BankSystem.Models;

public class Employee : Person
{
   public bool IsOwner { get; set; }
   public int Salary { get; set; }
   public string Contract { get; set; }
   public string Number { get; set; }
   public string PasNumber { get; set; }

   public Employee(string _fullName, bool _isOwner, int _salary, string _number, string _pasNumber)
   {
      FullName = _fullName;
      IsOwner = _isOwner;
      Salary = _salary;
      Number = _number;
      PasNumber = _pasNumber;
   }
   
}