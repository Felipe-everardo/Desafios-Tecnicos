using TechHelpSystem.Api.Models;
namespace TechHelpSystem.Api.Data;

using Microsoft.EntityFrameworkCore;

public class TechHelpContext : DbContext
{
    public TechHelpContext(DbContextOptions<TechHelpContext> options) : base(options)
    {
    }
    public DbSet<Chamado> Chamados { get; set; }
    public DbSet<Solicitante> Solicitantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Solicitante>(entity =>
        {
            entity.ToTable("Solicitantes");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(100);

            entity.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

            entity.HasIndex(e => e.Email)
            .IsUnique();

            entity.Property(e => e.CriadoEm)
                .IsRequired();
        });

        modelBuilder.Entity<Chamado>(entity =>
        {
            entity.ToTable("Chamados");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Descricao)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(e => e.Status);

            entity.Property(e => e.Prioridade)
            .IsRequired();

            entity.Property(e => e.CriadoEm);

            entity.Property(e => e.AtualizadoEm);

            entity.HasOne(e => e.Solicitante)
                .WithMany()
                .HasForeignKey(e => e.SolicitanteId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
