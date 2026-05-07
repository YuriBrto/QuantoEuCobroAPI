using Microsoft.EntityFrameworkCore;
using QuantoEuCobro.Domain.DTOs;
using QuantoEuCobro.Infrastructure;
using QuantoEuCobro.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuantoEuCobro.Services;

/// <summary>
/// Gerencia a listagem e consulta de templates de proposta.
/// </summary>
public class ServicoTemplate(AppDbContext contexto) : IServicoTemplate
{
    public async Task<List<TemplateDto>> ListarAsync(CancellationToken cancellationToken = default)
    {
        return await contexto.Templates
            .AsNoTracking()
            .OrderBy(t => t.Premium)
            .ThenBy(t => t.Nome)
            .Select(t => MapearParaDto(t))
            .ToListAsync(cancellationToken);
    }

    public async Task<TemplateDto> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var template = await contexto.Templates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException("Template não encontrado.");

        return MapearParaDto(template);
    }

    private static TemplateDto MapearParaDto(Domain.Entities.Template t)
        => new(t.Id, t.Nome, t.Categoria, t.CorPrimaria, t.CorSecundaria, t.Fonte, t.Premium, t.Thumbnail);
}