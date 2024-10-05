namespace BankSystem.App.Services.Exceptions;

public class UnderAgeClientException : Exception
{
    public UnderAgeClientException(string message) : base(message) { }
}