namespace QuantoEuCobro.Domain.DTOs;

// ── Dashboard ─────────────────────────────────────────────────────────────────

public record DashboardResumoDto(
    int TotalPropostas,
    int PropostasVisualizadas,
    int PropostasCriadasEsteMes,
    decimal TotalFaturadoEstimado
);

// ── Template ──────────────────────────────────────────────────────────────────

public record TemplateDto(
    int Id,
    string Nome,
    string Categoria,
    string CorPrimaria,
    string CorSecundaria,
    string Fonte,
    bool Premium,
    string? Thumbnail
);

// ── Resposta Padrão ───────────────────────────────────────────────────────────

public record RespostaPadraoDto<T>(
    bool Sucesso,
    string? Mensagem,
    T? Dados
)
{
    public static RespostaPadraoDto<T> Ok(T dados, string? mensagem = null)
        => new(true, mensagem, dados);

    public static RespostaPadraoDto<T> Erro(string mensagem)
        => new(false, mensagem, default);
}