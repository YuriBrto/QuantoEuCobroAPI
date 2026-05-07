using Microsoft.EntityFrameworkCore;
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
/// Gerencia criação, listagem, atualização e tracking de propostas.
/// </summary>
public class ServicoProposta(AppDbContext contexto) : IServicoProposta
{
    public async Task<PaginadoDto<PropostaDto>> ListarAsync(
        int usuarioId,
        PropostaFiltroDto filtro,
        CancellationToken cancellationToken = default)
    {
        var query = contexto.Propostas
            .AsNoTracking()
            .Include(p => p.Campos)
            .Where(p => p.UsuarioId == usuarioId && p.Ativo);

        // Filtro de busca por título ou nome do cliente
        if (!string.IsNullOrWhiteSpace(filtro.Busca))
        {
            var termo = filtro.Busca.ToLower();
            query = query.Where(p =>
                p.Titulo.ToLower().Contains(termo) ||
                (p.NomeCliente != null && p.NomeCliente.ToLower().Contains(termo)));
        }

        var total = await query.CountAsync(cancellationToken);

        var itens = await query
            .OrderByDescending(p => p.DataCriacao)
            .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina)
            .Take(filtro.TamanhoPagina)
            .Select(p => MapearParaDto(p))
            .ToListAsync(cancellationToken);

        return new PaginadoDto<PropostaDto>(
            itens,
            total,
            filtro.Pagina,
            filtro.TamanhoPagina,
            (int)Math.Ceiling((double)total / filtro.TamanhoPagina)
        );
    }

    public async Task<PropostaDto> ObterPorIdAsync(
        int id,
        int usuarioId,
        CancellationToken cancellationToken = default)
    {
        var proposta = await ObterComValidacaoDeOwnershipAsync(id, usuarioId, cancellationToken);
        return MapearParaDto(proposta);
    }

    public async Task<PropostaDto> CriarAsync(
     int usuarioId,
     CriarPropostaDto dto,
     CancellationToken cancellationToken = default)
    {
        var template = dto.TemplateId.HasValue
            ? await contexto.Templates.FirstOrDefaultAsync(t => t.Id == dto.TemplateId, cancellationToken)
            : null;

        var proposta = new Proposta
        {
            UsuarioId = usuarioId,
            Titulo = dto.Titulo.Trim(),
            DescricaoCliente = dto.DescricaoCliente?.Trim(),
            NomeCliente = dto.NomeCliente?.Trim(),
            EmailCliente = dto.EmailCliente?.ToLower().Trim(),
            ValorTotal = dto.ValorTotal,
            DataValidade = dto.DataValidade,
            TemplateId = dto.TemplateId,
            PublicToken = Guid.NewGuid().ToString("N"),
            DataCriacao = DateTime.UtcNow,
            Ativo = true,

            // 🔥 AQUI o diferencial entra corretamente
            LayoutRenderizado = template != null
                ? TemplateEngine.Aplicar(template.LayoutJson, dto)
                : null
        };

        // Adiciona campos personalizados
        if (dto.Campos?.Count > 0)
        {
            proposta.Campos = dto.Campos.Select(c => new CampoPersonalizado
            {
                Titulo = c.Titulo.Trim(),
                Valor = c.Valor.Trim(),
                TipoCampo = c.TipoCampo,
                Ordem = c.Ordem
            }).ToList();
        }

        contexto.Propostas.Add(proposta);
        await contexto.SaveChangesAsync(cancellationToken);

        return MapearParaDto(proposta);
    }

    public async Task<PropostaDto> AtualizarAsync(
        int id,
        int usuarioId,
        AtualizarPropostaDto dto,
        CancellationToken cancellationToken = default)
    {
        var proposta = await ObterComValidacaoDeOwnershipAsync(id, usuarioId, cancellationToken);

        proposta.Titulo = dto.Titulo.Trim();
        proposta.DescricaoCliente = dto.DescricaoCliente?.Trim();
        proposta.NomeCliente = dto.NomeCliente?.Trim();
        proposta.EmailCliente = dto.EmailCliente?.ToLower().Trim();
        proposta.ValorTotal = dto.ValorTotal;
        proposta.DataValidade = dto.DataValidade;
        proposta.TemplateId = dto.TemplateId;

        // Substitui campos personalizados
        var camposAntigos = await contexto.CamposPersonalizados
            .Where(c => c.PropostaId == id)
            .ToListAsync(cancellationToken);

        contexto.CamposPersonalizados.RemoveRange(camposAntigos);

        if (dto.Campos?.Count > 0)
        {
            proposta.Campos = dto.Campos.Select(c => new CampoPersonalizado
            {
                Titulo = c.Titulo.Trim(),
                Valor = c.Valor.Trim(),
                TipoCampo = c.TipoCampo,
                Ordem = c.Ordem
            }).ToList();
        }

        await contexto.SaveChangesAsync(cancellationToken);
        return MapearParaDto(proposta);
    }

    public async Task ExcluirAsync(
        int id,
        int usuarioId,
        CancellationToken cancellationToken = default)
    {
        var proposta = await ObterComValidacaoDeOwnershipAsync(id, usuarioId, cancellationToken);

        // Soft delete — mantém histórico
        proposta.Ativo = false;
        await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<PropostaDto> ObterPorTokenPublicoAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var proposta = await contexto.Propostas
            .Include(p => p.Campos)
            .FirstOrDefaultAsync(p => p.PublicToken == token && p.Ativo, cancellationToken)
            ?? throw new KeyNotFoundException("Proposta não encontrada ou link inválido.");

        // Incrementa visualizações automaticamente
        proposta.QuantidadeVisualizacoes++;
        proposta.UltimaVisualizacao = DateTime.UtcNow;
        await contexto.SaveChangesAsync(cancellationToken);

        return MapearParaDto(proposta);
    }

    public async Task<DashboardResumoDto> ObterResumoAsync(
        int usuarioId,
        CancellationToken cancellationToken = default)
    {
        var inicioDoMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        var propostas = await contexto.Propostas
            .AsNoTracking()
            .Where(p => p.UsuarioId == usuarioId && p.Ativo)
            .ToListAsync(cancellationToken);

        return new DashboardResumoDto(
            TotalPropostas: propostas.Count,
            PropostasVisualizadas: propostas.Count(p => p.QuantidadeVisualizacoes > 0),
            PropostasCriadasEsteMes: propostas.Count(p => p.DataCriacao >= inicioDoMes),
            TotalFaturadoEstimado: propostas.Sum(p => p.ValorTotal)
        );
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Busca a proposta e valida que pertence ao usuário autenticado.
    /// </summary>
    private async Task<Proposta> ObterComValidacaoDeOwnershipAsync(
        int id,
        int usuarioId,
        CancellationToken cancellationToken)
    {
        var proposta = await contexto.Propostas
            .Include(p => p.Campos)
            .FirstOrDefaultAsync(p => p.Id == id && p.Ativo, cancellationToken)
            ?? throw new KeyNotFoundException("Proposta não encontrada.");

        if (proposta.UsuarioId != usuarioId)
            throw new UnauthorizedAccessException("Acesso negado a esta proposta.");

        return proposta;
    }

    private static PropostaDto MapearParaDto(Proposta p)
        => new(
            p.Id,
            p.Titulo,
            p.NomeCliente,
            p.EmailCliente,
            p.ValorTotal,
            p.DataCriacao,
            p.DataValidade,
            p.PublicToken,
            p.QuantidadeVisualizacoes,
            p.UltimaVisualizacao,
            p.TemplateId,
            p.Campos
                .OrderBy(c => c.Ordem)
                .Select(c => new CampoDto(c.Id, c.Titulo, c.Valor, c.TipoCampo, c.Ordem))
                .ToList()
        );






    public async Task<PropostaPreviewDto> ObterPreviewPublicoAsync(
    string token,
    CancellationToken cancellationToken = default)
    {
        var proposta = await contexto.Propostas
            .Include(p => p.Campos)
            .FirstOrDefaultAsync(p => p.PublicToken == token && p.Ativo, cancellationToken)
            ?? throw new KeyNotFoundException("Proposta não encontrada.");

        proposta.QuantidadeVisualizacoes++;
        proposta.UltimaVisualizacao = DateTime.UtcNow;

        await contexto.SaveChangesAsync(cancellationToken);

        return new PropostaPreviewDto(
            proposta.Id,
            proposta.Titulo,
            proposta.NomeCliente,
            proposta.EmailCliente,
            proposta.ValorTotal,
            proposta.DataCriacao,
            proposta.DataValidade,
            proposta.LayoutRenderizado,
            proposta.Campos
                .OrderBy(c => c.Ordem)
                .Select(c => new CampoDto(c.Id, c.Titulo, c.Valor, c.TipoCampo, c.Ordem))
                .ToList()
        );
    }








}