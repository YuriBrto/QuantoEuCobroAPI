using Microsoft.EntityFrameworkCore;
using QuantoEuCobro.Domain.Entities;

namespace QuantoEuCobro.Infrastructure;

/// <summary>
/// Contexto principal do banco de dados da aplicação.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> opcoes) : DbContext(opcoes)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Proposta> Propostas => Set<Proposta>();
    public DbSet<CampoPersonalizado> CamposPersonalizados => Set<CampoPersonalizado>();
    public DbSet<Template> Templates => Set<Template>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Usuario ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Usuario>(entidade =>
        {
            entidade.HasKey(u => u.Id);
            entidade.HasIndex(u => u.Email).IsUnique();
            entidade.Property(u => u.Nome).HasMaxLength(100).IsRequired();
            entidade.Property(u => u.Email).HasMaxLength(200).IsRequired();
            entidade.Property(u => u.SenhaHash).IsRequired();
        });

        // ── Proposta ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Proposta>(entidade =>
        {
            entidade.HasKey(p => p.Id);
            entidade.HasIndex(p => p.PublicToken).IsUnique();
            entidade.Property(p => p.Titulo).HasMaxLength(200).IsRequired();
            entidade.Property(p => p.ValorTotal).HasColumnType("decimal(18,2)");

            // Relacionamento com Usuario
            entidade.HasOne(p => p.Usuario)
                .WithMany(u => u.Propostas)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Template (opcional)
            entidade.HasOne(p => p.Template)
                .WithMany(t => t.Propostas)
                .HasForeignKey(p => p.TemplateId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ── CampoPersonalizado ────────────────────────────────────────────────
        modelBuilder.Entity<CampoPersonalizado>(entidade =>
        {
            entidade.HasKey(c => c.Id);
            entidade.Property(c => c.Titulo).HasMaxLength(100).IsRequired();
            entidade.Property(c => c.Valor).HasMaxLength(2000).IsRequired();

            entidade.HasOne(c => c.Proposta)
                .WithMany(p => p.Campos)
                .HasForeignKey(c => c.PropostaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Template ──────────────────────────────────────────────────────────
        modelBuilder.Entity<Template>(entidade =>
        {
            entidade.HasKey(t => t.Id);
            entidade.Property(t => t.Nome).HasMaxLength(100).IsRequired();
            entidade.Property(t => t.Categoria).HasMaxLength(50).IsRequired();
            entidade.Property(t => t.CorPrimaria).HasMaxLength(10);
            entidade.Property(t => t.CorSecundaria).HasMaxLength(10);
            entidade.Property(t => t.Fonte).HasMaxLength(50);
        });

        // ── Seed de templates padrão ──────────────────────────────────────────
        modelBuilder.Entity<Template>().HasData(
            new Template
            {
                Id = 1,
                Nome = "Moderno",
                Categoria = "Tecnologia",
                CorPrimaria = "#6C63FF",
                CorSecundaria = "#F3F4F6",
                Fonte = "Poppins",
                Premium = false,
                Thumbnail = "/thumbnails/moderno.png"
            },
            new Template
            {
                Id = 2,
                Nome = "Elegante",
                Categoria = "Design",
                CorPrimaria = "#1A1A2E",
                CorSecundaria = "#E8C547",
                Fonte = "Playfair Display",
                Premium = false,
                Thumbnail = "/thumbnails/elegante.png"
            },
            new Template
            {
                Id = 3,
                Nome = "Premium Pro",
                Categoria = "Agência",
                CorPrimaria = "#0F172A",
                CorSecundaria = "#38BDF8",
                Fonte = "Montserrat",
                Premium = true,
                Thumbnail = "/thumbnails/premium.png"
            }
        );
    }
}