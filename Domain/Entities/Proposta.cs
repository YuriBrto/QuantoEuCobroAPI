namespace QuantoEuCobro.Domain.Entities;

/// <summary>
/// Proposta comercial criada por um freelancer.
/// </summary>
public class Proposta
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int? TemplateId { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string? DescricaoCliente { get; set; }
    public string? NomeCliente { get; set; }
    public string? EmailCliente { get; set; }

    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataValidade { get; set; }

    // Tracking público
    public string PublicToken { get; set; } = Guid.NewGuid().ToString("N");
    public int QuantidadeVisualizacoes { get; set; } = 0;
    public DateTime? UltimaVisualizacao { get; set; }

    public bool Ativo { get; set; } = true;

  public string? LayoutRenderizado { get; set; }

    // Navegação
    public Usuario Usuario { get; set; } = null!;
    public Template? Template { get; set; }
    public ICollection<CampoPersonalizado> Campos { get; set; } = [];
}