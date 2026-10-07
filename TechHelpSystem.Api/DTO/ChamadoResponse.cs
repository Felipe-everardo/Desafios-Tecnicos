using TechHelpSystem.Api.Models;
using TechHelpSystem.Api.Models.Enums;

namespace TechHelpSystem.Api.DTO;
public class ChamadoResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Prioridade Prioridade { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
    public string NomeSolicitante { get; set; } = string.Empty;
}