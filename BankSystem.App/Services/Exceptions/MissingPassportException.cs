namespace BankSystem.App.Services.Exceptions;

public class MissingPassportException : Exception
{
    public MissingPassportException(string message) : base(message) { }
}