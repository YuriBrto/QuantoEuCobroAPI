using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantoEuCobro.Domain.DTOs;
using QuantoEuCobro.Extensions;
using QuantoEuCobro.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace QuantoEuCobro.Controllers;

/// <summary>
/// Gerencia autenticação: registro, login e dados do usuário atual.
/// </summary>
[ApiController]
[Route("auth")]
[Produces("application/json")]
public class AuthController(IServicoAutenticacao servicoAutenticacao) : ControllerBase
{
    /// <summary>Registra um novo freelancer na plataforma.</summary>
    /// <response code="201">Conta criada com sucesso. Retorna o token JWT.</response>
    /// <response code="400">Dados inválidos ou e-mail já cadastrado.</response>
    [HttpPost("registrar")]
    [ProducesResponseType(typeof(RespostaPadraoDto<TokenRespostaDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistrarUsuarioDto dto,
        CancellationToken cancellationToken)
    {
        var resultado = await servicoAutenticacao.RegistrarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(Me), RespostaPadraoDto<TokenRespostaDto>.Ok(resultado, "Conta criada com sucesso!"));
    }

    /// <summary>Autentica um freelancer e retorna o token JWT.</summary>
    /// <response code="200">Login realizado com sucesso.</response>
    /// <response code="401">Credenciais inválidas.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(RespostaPadraoDto<TokenRespostaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginDto dto,
        CancellationToken cancellationToken)
    {
        var resultado = await servicoAutenticacao.LoginAsync(dto, cancellationToken);
        return Ok(RespostaPadraoDto<TokenRespostaDto>.Ok(resultado));
    }

    /// <summary>Retorna os dados do usuário autenticado.</summary>
    /// <response code="200">Dados do usuário atual.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(RespostaPadraoDto<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var usuarioId = User.ObterUsuarioId();
        var usuario = await servicoAutenticacao.ObterUsuarioAtualAsync(usuarioId, cancellationToken);
        return Ok(RespostaPadraoDto<UsuarioDto>.Ok(usuario));
    }
}