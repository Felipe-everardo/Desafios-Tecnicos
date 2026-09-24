using System.ComponentModel.DataAnnotations;

namespace TechHelpSystem.Api.DTO;

public class SolicitanteRequest
{
    [Required(ErrorMessage = "Nome is required")]
    [Length(3,100, ErrorMessage = "Nome must be between 3 and 100 characters")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public  string Email { get; set; } = string.Empty;
}
