using System;

namespace QuantoEuCobro.Domain.DTOs;

// ── Registro ──────────────────────────────────────────────────────────────────

public record RegistrarUsuarioDto(
    string Nome,
    string Email,
    string Senha
);

// ── Login ─────────────────────────────────────────────────────────────────────

public record LoginDto(
    string Email,
    string Senha
);

// ── Respostas ─────────────────────────────────────────────────────────────────

public record TokenRespostaDto(
    string Token,
    DateTime Expiracao,
    UsuarioDto Usuario
);

public record UsuarioDto(
    int Id,
    string Nome,
    string Email,
    DateTime DataCriacao,
    DateTime? UltimoLogin
);