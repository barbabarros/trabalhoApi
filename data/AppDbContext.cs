using Microsoft.EntityFrameworkCore;
using templateMariaDb.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Estado> Estados { get; set; } = null!;
    public DbSet<Cidade> Cidades { get; set; } = null!;
    public DbSet<LinhaOnibus> LinhasOnibus { get; set; } = null!;
    public DbSet<Onibus> Onibus { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cidade>()
            .HasOne(c => c.Estado)
            .WithMany(e => e.Cidades)
            .HasForeignKey(c => c.EstadoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LinhaOnibus>()
            .HasOne(l => l.Cidade)
            .WithMany(c => c.LinhasOnibus)
            .HasForeignKey(l => l.CidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Onibus>()
            .HasOne(o => o.LinhaOnibus)
            .WithMany(l => l.Onibus)
            .HasForeignKey(o => o.LinhaOnibusId)
            .OnDelete(DeleteBehavior.Restrict);
            
        base.OnModelCreating(modelBuilder);
    }
}
    