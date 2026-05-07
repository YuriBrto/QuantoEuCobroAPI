using System.Collections.Generic;

public class Template
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string CorPrimaria { get; set; } = string.Empty;
    public string CorSecundaria { get; set; } = string.Empty;
    public string Fonte { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    // 👇 DIFERENCIAL REAL
    public string LayoutJson { get; set; } = string.Empty;

    public bool Premium { get; set; }
    public string? Thumbnail { get; set; }

    public ICollection<Proposta> Propostas { get; set; } = [];
}