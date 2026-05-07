using QuantoEuCobro.Domain.Enums;
using System;
using System.Collections.Generic;

namespace QuantoEuCobro.Domain.DTOs;

// ── Criação ───────────────────────────────────────────────────────────────────

public record CriarPropostaDto(
	string Titulo,
	string? DescricaoCliente,
	string? NomeCliente,
	string? EmailCliente,
	decimal ValorTotal,
	DateTime? DataValidade,
	int? TemplateId,
	List<CriarCampoDto>? Campos
);

public record CriarCampoDto(
	string Titulo,
	string Valor,
	TipoCampo TipoCampo,
	int Ordem
);

// ── Atualização ───────────────────────────────────────────────────────────────

public record AtualizarPropostaDto(
	string Titulo,
	string? DescricaoCliente,
	string? NomeCliente,
	string? EmailCliente,
	decimal ValorTotal,
	DateTime? DataValidade,
	int? TemplateId,
	List<CriarCampoDto>? Campos
);

// ── Respostas ─────────────────────────────────────────────────────────────────

public record PropostaDto(
	int Id,
	string Titulo,
	string? NomeCliente,
	string? EmailCliente,
	decimal ValorTotal,
	DateTime DataCriacao,
	DateTime? DataValidade,
	string PublicToken,
	int QuantidadeVisualizacoes,
	DateTime? UltimaVisualizacao,
	int? TemplateId,
	List<CampoDto> Campos
);

public record CampoDto(
	int Id,
	string Titulo,
	string Valor,
	TipoCampo TipoCampo,
	int Ordem
);

// ── Paginação ─────────────────────────────────────────────────────────────────

public record PropostaFiltroDto(
	string? Busca,
	int Pagina = 1,
	int TamanhoPagina = 10
);

public record PaginadoDto<T>(
	List<T> Itens,
	int Total,
	int Pagina,
	int TamanhoPagina,
	int TotalPaginas
);