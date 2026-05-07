using QuantoEuCobro.Domain.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace QuantoEuCobro.Services.Interfaces;

public interface IServicoProposta
{
    Task<PropostaPreviewDto> ObterPreviewPublicoAsync(string token, CancellationToken cancellationToken = default);
    Task<PaginadoDto<PropostaDto>> ListarAsync(int usuarioId, PropostaFiltroDto filtro, CancellationToken cancellationToken = default);
    Task<PropostaDto> ObterPorIdAsync(int id, int usuarioId, CancellationToken cancellationToken = default);
    Task<PropostaDto> CriarAsync(int usuarioId, CriarPropostaDto dto, CancellationToken cancellationToken = default);
    Task<PropostaDto> AtualizarAsync(int id, int usuarioId, AtualizarPropostaDto dto, CancellationToken cancellationToken = default);
    Task ExcluirAsync(int id, int usuarioId, CancellationToken cancellationToken = default);
    Task<PropostaDto> ObterPorTokenPublicoAsync(string token, CancellationToken cancellationToken = default);
    Task<DashboardResumoDto> ObterResumoAsync(int usuarioId, CancellationToken cancellationToken = default);
}