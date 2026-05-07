using QuantoEuCobro.Domain.Entities;
using System;

namespace QuantoEuCobro.Authentication;

/// <summary>
/// Contrato para geração de tokens JWT.
/// </summary>
public interface IServicoToken
{
    string GerarToken(Usuario usuario);
    DateTime ObterExpiracao();
}