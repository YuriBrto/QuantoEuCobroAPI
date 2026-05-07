using Microsoft.EntityFrameworkCore;
using QuantoEuCobro.Authentication;
using QuantoEuCobro.Domain.DTOs;
using QuantoEuCobro.Domain.Entities;
using QuantoEuCobro.Infrastructure;
using QuantoEuCobro.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuantoEuCobro.Services;

/// <summary>
/// Gerencia registro, login e consulta do usuário autenticado.
/// </summary>
public class ServicoAutenticacao(
    AppDbContext contexto,
    IServicoToken servicoToken) : IServicoAutenticacao
{
    public async Task<TokenRespostaDto> RegistrarAsync(
        RegistrarUsuarioDto dto,
        CancellationToken cancellationToken = default)
    {
        // Verifica se e-mail já está em uso
        var emailEmUso = await contexto.Usuarios
            .AsNoTracking()
            .AnyAsync(u => u.Email == dto.Email.ToLower(), cancellationToken);

        if (emailEmUso)
            throw new InvalidOperationException("Este e-mail já está cadastrado.");

        var usuario = new Usuario
        {
            Nome = dto.Nome.Trim(),
            Email = dto.Email.ToLower().Trim(),
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        contexto.Usuarios.Add(usuario);
        await contexto.SaveChangesAsync(cancellationToken);

        return GerarResposta(usuario);
    }

    public async Task<TokenRespostaDto> LoginAsync(
        LoginDto dto,
        CancellationToken cancellationToken = default)
    {
        var usuario = await contexto.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower(), cancellationToken)
            ?? throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Conta desativada. Entre em contato com o suporte.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        // Atualiza último login
        usuario.UltimoLogin = DateTime.UtcNow;
        await contexto.SaveChangesAsync(cancellationToken);

        return GerarResposta(usuario);
    }

    public async Task<UsuarioDto> ObterUsuarioAtualAsync(
        int usuarioId,
        CancellationToken cancellationToken = default)
    {
        var usuario = await contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == usuarioId, cancellationToken)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");

        return MapearParaDto(usuario);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private TokenRespostaDto GerarResposta(Usuario usuario)
    {
        var token = servicoToken.GerarToken(usuario);
        var expiracao = servicoToken.ObterExpiracao();
        return new TokenRespostaDto(token, expiracao, MapearParaDto(usuario));
    }

    private static UsuarioDto MapearParaDto(Usuario usuario)
        => new(usuario.Id, usuario.Nome, usuario.Email, usuario.DataCriacao, usuario.UltimoLogin);
}