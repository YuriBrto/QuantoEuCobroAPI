using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantoEuCobro.Domain.DTOs;
using QuantoEuCobro.Services.Interfaces;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace QuantoEuCobro.Controllers;

[ApiController]
[Route("api/propostas")]
[Authorize]
public class PropostasController : ControllerBase
{
    private readonly IServicoProposta _servico;

    public PropostasController(IServicoProposta servico)
    {
        _servico = servico;
    }

    // 🔹 LISTAR
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] PropostaFiltroDto filtro, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var resultado = await _servico.ListarAsync(usuarioId, filtro, ct);

        return Ok(resultado);
    }

    // 🔹 OBTER POR ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var resultado = await _servico.ObterPorIdAsync(id, usuarioId, ct);

        return Ok(resultado);
    }

    // 🔹 CRIAR (COM TEMPLATE)
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPropostaDto dto, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var resultado = await _servico.CriarAsync(usuarioId, dto, ct);

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
    }

    // 🔹 ATUALIZAR
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarPropostaDto dto, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var resultado = await _servico.AtualizarAsync(id, usuarioId, dto, ct);

        return Ok(resultado);
    }

    // 🔹 DELETE (SOFT DELETE)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        await _servico.ExcluirAsync(id, usuarioId, ct);

        return NoContent();
    }

    // 🔹 RESUMO DASHBOARD
    [HttpGet("resumo")]
    public async Task<IActionResult> Resumo(CancellationToken ct)
    {
        var usuarioId = ObterUsuarioId();
        var resultado = await _servico.ObterResumoAsync(usuarioId, ct);

        return Ok(resultado);
    }

    // 🔹 PROPOSTA PÚBLICA (link)
    [HttpGet("publico/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> Publico(string token, CancellationToken ct)
    {
        var resultado = await _servico.ObterPorTokenPublicoAsync(token, ct);

        return Ok(resultado);
    }


    [HttpGet("preview/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> Preview(string token, CancellationToken ct)
    {
        var resultado = await _servico.ObterPreviewPublicoAsync(token, ct);
        return Ok(resultado);
    }

    // 🔐 helper
    private int ObterUsuarioId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}