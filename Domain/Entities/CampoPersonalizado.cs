using QuantoEuCobro.Domain.Enums;

namespace QuantoEuCobro.Domain.Entities;

/// <summary>
/// Campo dinâmico de uma proposta (escopo, tecnologias, prazo etc.).
/// </summary>
public class CampoPersonalizado
{
    public int Id { get; set; }
    public int PropostaId { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public TipoCampo TipoCampo { get; set; } = TipoCampo.Texto;
    public int Ordem { get; set; } = 0;

    // Navegação
    public Proposta Proposta { get; set; } = null!;
}