using System;
using System.Collections.Generic;

namespace QuantoEuCobro.Domain.Entities;

/// <summary>
/// Representa um freelancer cadastrado na plataforma.
/// </summary>
public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoLogin { get; set; }
    public bool Ativo { get; set; } = true;

    // Navegação
    public ICollection<Proposta> Propostas { get; set; } = [];
}