using Microsoft.IdentityModel.Tokens;
using QuantoEuCobro.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuantoEuCobro.Authentication;

/// <summary>
/// Serviço responsável por gerar e configurar tokens JWT.
/// </summary>
public class ServicoToken(IConfiguration configuracao) : IServicoToken
{
    private readonly string _chaveSecreta = configuracao["Jwt:ChaveSecreta"]
        ?? throw new InvalidOperationException("Chave JWT não configurada.");

    private readonly int _expiracaoEmHoras = int.Parse(
        configuracao["Jwt:ExpiracaoEmHoras"] ?? "8");

    public string GerarToken(Usuario usuario)
    {
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_chaveSecreta));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        // Claims do usuário autenticado
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim("usuario_id", usuario.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuracao["Jwt:Issuer"],
            audience: configuracao["Jwt:Audience"],
            claims: claims,
            expires: ObterExpiracao(),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime ObterExpiracao()
        => DateTime.UtcNow.AddHours(_expiracaoEmHoras);
}