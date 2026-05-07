using QuantoEuCobro.Domain.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace QuantoEuCobro.Services.Interfaces;

public interface IServicoAutenticacao
{
    Task<TokenRespostaDto> RegistrarAsync(RegistrarUsuarioDto dto, CancellationToken cancellationToken = default);
    Task<TokenRespostaDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task<UsuarioDto> ObterUsuarioAtualAsync(int usuarioId, CancellationToken cancellationToken = default);
}