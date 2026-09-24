namespace TechHelpSystem.Api.Exceptions;

public class EmailDuplicadoException : Exception
{
    public EmailDuplicadoException(string email)
        : base($"O email '{email}' já está em uso.")
    {
    }
}
