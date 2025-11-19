namespace ElearningPlatform.Core.Exceptions;

public class EmailExistsException : Exception
{
    public EmailExistsException(string email) 
        : base($"Email '{email}' is already in use.")
    {
    }
}