namespace TechHelpSystem.Api.Data;
using TechHelpSystem.Api.Models;

using Microsoft.EntityFrameworkCore;

public class TechHelpContext : DbContext
{
    public TechHelpContext(DbContextOptions<TechHelpContext> options) : base(options)
    {
    }

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
    }
}
