namespace BankSystem.App.DTO;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public bool IsOwner { get; set; }
    public int Salary { get; set; }
    public string Contract { get; set; }
    public string PhoneNumber { get; set; }
    public string PasNumber { get; set; }
}