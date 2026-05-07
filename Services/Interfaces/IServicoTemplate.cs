using QuantoEuCobro.Domain.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuantoEuCobro.Services.Interfaces;

public interface IServicoTemplate
{
    Task<List<TemplateDto>> ListarAsync(CancellationToken cancellationToken = default);
    Task<TemplateDto> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
}