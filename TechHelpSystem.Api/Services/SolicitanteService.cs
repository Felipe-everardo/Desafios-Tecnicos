using Microsoft.EntityFrameworkCore;
using TechHelpSystem.Api.Data;
using TechHelpSystem.Api.DTO;
using TechHelpSystem.Api.Exceptions;
using TechHelpSystem.Api.Models;

namespace TechHelpSystem.Api.Services;

public class SolicitanteService
{
    private readonly TechHelpContext _context;

    public SolicitanteService(TechHelpContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SolicitanteResponse>> GetAllSolicitantesAsync(CancellationToken cancellationToken)
    {
        return await _context.Solicitantes
            .AsNoTracking()
            .Select(s => new SolicitanteResponse
            {
                Id = s.Id,
                Nome = s.Nome,
                Email = s.Email,
                CriadoEm = s.CriadoEm
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<SolicitanteResponse?> GetSolicitanteByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Solicitantes
                    .AsNoTracking()
                    .Where(s => s.Id == id)
                    .Select(s => new SolicitanteResponse
                    {
                        Id = s.Id,
                        Nome = s.Nome,
                        Email = s.Email,
                        CriadoEm = s.CriadoEm
                    })
                    .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SolicitanteResponse> CreateSolicitanteAsync(SolicitanteRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var novoSolicitante = new Solicitante(request.Nome, request.Email);
   
        var existeEmail = await _context.Solicitantes.AnyAsync(s => s.Email == novoSolicitante.Email, cancellationToken);

        if (existeEmail)
            throw new EmailDuplicadoException(novoSolicitante.Email);

        _context.Solicitantes.Add(novoSolicitante);
        await _context.SaveChangesAsync(cancellationToken);

        return new SolicitanteResponse
        {
            Id = novoSolicitante.Id,
            Nome = novoSolicitante.Nome,
            Email = novoSolicitante.Email,
            CriadoEm = novoSolicitante.CriadoEm
        };
    }
}
