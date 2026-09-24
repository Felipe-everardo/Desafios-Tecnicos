namespace TechHelpSystem.Api.DTO;

public class SolicitanteResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset CriadoEm { get; set; }
}
