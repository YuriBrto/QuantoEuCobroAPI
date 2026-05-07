using System;
using System.Collections.Generic;

namespace QuantoEuCobro.Domain.DTOs;

public record PropostaPreviewDto(
    int Id,
    string Titulo,
    string? NomeCliente,
    string? EmailCliente,
    decimal ValorTotal,
    DateTime DataCriacao,
    DateTime? DataValidade,
    string? LayoutRenderizado,
    List<CampoDto> Campos
);