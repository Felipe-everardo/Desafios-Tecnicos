namespace TechHelpSystem.Api.Models;

public class Solicitante
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public DateTimeOffset CriadoEm { get; private set; }

    public Solicitante(string nome, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Nome cannot be null or whitespace", nameof(nome));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or whitespace", nameof(email));
        }

        Nome = nome.Trim();
        Email = email.Trim().ToLowerInvariant();
        CriadoEm = DateTimeOffset.UtcNow;
    }
}
