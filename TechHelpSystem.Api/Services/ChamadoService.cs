using TechHelpSystem.Api.Data;
using TechHelpSystem.Api.Models;
using Microsoft.EntityFrameworkCore;
using TechHelpSystem.Api.DTO;

namespace TechHelpSystem.Api.Services;

public class ChamadoService
{
    private readonly TechHelpContext _context;

    public ChamadoService(TechHelpContext context)
    {
        _context = context;
    }

    public async Task<ChamadoResponse> CreateChamadoAsync(ChamadoRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var solicitante = await _context.Solicitantes
            .FirstOrDefaultAsync(s => s.Id == request.SolicitanteId, cancellationToken);

        if (solicitante is null)
        {
            throw new ArgumentException("Solicitante não encontrado");
        }

        var chamado = new Chamado(
            request.Titulo, 
            request.Descricao, 
            request.Prioridade, 
            request.SolicitanteId
        );
        
        _context.Chamados.Add(chamado);
        await _context.SaveChangesAsync(cancellationToken);

        return new ChamadoResponse
        {
            Id = chamado.Id,
            Titulo = chamado.Titulo,
            Status = chamado.Status,
            NomeSolicitante = chamado.Solicitante.Nome,
            CriadoEm = chamado.CriadoEm
        };
    }

    public async Task<ChamadoResponse?> GetChamadoByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Chamados
                .Include(c => c.Solicitante)
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new ChamadoResponse
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Status = c.Status,
                    NomeSolicitante = c.Solicitante.Nome,
                    CriadoEm = c.CriadoEm
                })
                .FirstOrDefaultAsync(cancellationToken);
    }
}
