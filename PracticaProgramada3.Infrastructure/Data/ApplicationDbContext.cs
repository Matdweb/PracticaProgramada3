using Microsoft.EntityFrameworkCore;
using PracticaProgramada3.Domain.Entities;

namespace PracticaProgramada3.Infrastructure.Data;

/// <summary>
/// Contexto de Entity Framework: conecta las entidades de Domain con las tablas de SQL Server.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Votante> Votantes { get; set; }

    public DbSet<Partido> Partidos { get; set; }

    public DbSet<Voto> Votos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Votante>(entity =>
        {
            entity.Property(v => v.Cedula).IsRequired().HasMaxLength(9);
            entity.Property(v => v.NombreCompleto).IsRequired().HasMaxLength(150);

            // No se repiten cédulas
            entity.HasIndex(v => v.Cedula).IsUnique();
        });

        builder.Entity<Partido>(entity =>
        {
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            entity.HasIndex(p => p.Nombre).IsUnique();
        });

        builder.Entity<Voto>(entity =>
        {
            // Un votante solo puede tener un voto: es la defensa final de la regla "una cédula, un voto",
            // incluso si dos solicitudes llegan al mismo tiempo.
            entity.HasIndex(v => v.VotanteId).IsUnique();

            // Restrict: no se puede borrar un votante ni un partido que ya tiene votos
            entity.HasOne<Votante>()
                .WithMany()
                .HasForeignKey(v => v.VotanteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Partido>()
                .WithMany()
                .HasForeignKey(v => v.PartidoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
