namespace BankSystem.Models;

public class Client : Person
{
    public string Number { get; set; }
    public string PasNumber { get; set; }

    public Client(string _fullName, string  _number, string _pasNumber, int _age)
    {
        FullName = _fullName;
        Number = _number;
        PasNumber = _pasNumber;
        Age = _age;
    }
}