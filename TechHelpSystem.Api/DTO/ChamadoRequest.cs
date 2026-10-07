using System.ComponentModel.DataAnnotations;
using TechHelpSystem.Api.Models;
using TechHelpSystem.Api.Models.Enums;

namespace TechHelpSystem.Api.DTO;

public class ChamadoRequest
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public Prioridade Prioridade { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "SolicitanteId must be a positive integer")]
    public int SolicitanteId { get; set; }
}
