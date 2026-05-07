using System;
using System.Security.Claims;

namespace QuantoEuCobro.Extensions;

/// <summary>
/// Extensões para facilitar acesso às claims do usuário autenticado.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Retorna o Id do usuário autenticado a partir das claims do JWT.
    /// Lança exceção se não encontrado.
    /// </summary>
    public static int ObterUsuarioId(this ClaimsPrincipal usuario)
    {
        var claim = usuario.FindFirst(ClaimTypes.NameIdentifier)
            ?? usuario.FindFirst("usuario_id")
            ?? throw new UnauthorizedAccessException("Usuário não autenticado.");

        return int.Parse(claim.Value);
    }
}