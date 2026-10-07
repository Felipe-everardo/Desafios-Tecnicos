using TechHelpSystem.Api.Models.Enums;

namespace TechHelpSystem.Api.Models;

public class Chamado
{
    public int Id { get; set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public Status Status { get; private set; }
    public Prioridade Prioridade { get; private set; }
    public int SolicitanteId { get; private set; }
    public DateTimeOffset CriadoEm { get; private set; }
    public DateTimeOffset AtualizadoEm { get; private set; }

    public Solicitante Solicitante { get; private set; }

    public Chamado(string titulo, string descricao, Prioridade prioridade, int solicitanteId)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("Titulo cannot be null or whitespace", nameof(titulo));
        }
        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("Descricao cannot be null or whitespace", nameof(descricao));
        }
        if (solicitanteId <= 0)
        {
            throw new ArgumentException("Solicitante inválido", nameof(solicitanteId));
        }

        Titulo = titulo.Trim();
        Descricao = descricao.Trim();
        Status = Status.Aberto;
        Prioridade = prioridade;
        SolicitanteId = solicitanteId;
        CriadoEm = DateTimeOffset.UtcNow;
    }

    protected Chamado() { }

    public void AtualizarStatus(Status novoStatus)
    {
        if (novoStatus == Status)
        {
            throw new InvalidOperationException("O status do chamado já é o mesmo.");
        }
        Status = novoStatus;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }
}
