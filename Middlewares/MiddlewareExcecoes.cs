using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuantoEuCobro.Middlewares;

/// <summary>
/// Intercepta exceções não tratadas e retorna respostas padronizadas.
/// Evita que stack traces vazem para o cliente em produção.
/// </summary>
public class MiddlewareDeExcecoes(RequestDelegate proximo, ILogger<MiddlewareDeExcecoes> logger)
{
    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await proximo(contexto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exceção não tratada: {Mensagem}", ex.Message);
            await TratarExcecaoAsync(contexto, ex);
        }
    }

    private static Task TratarExcecaoAsync(HttpContext contexto, Exception excecao)
    {
        var (statusCode, mensagem) = excecao switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, excecao.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, excecao.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, excecao.Message),
            ArgumentException => (HttpStatusCode.BadRequest, excecao.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro interno. Tente novamente.")
        };

        var resposta = new
        {
            sucesso = false,
            mensagem,
            dados = (object?)null
        };

        contexto.Response.ContentType = "application/json";
        contexto.Response.StatusCode = (int)statusCode;

        return contexto.Response.WriteAsync(
            JsonSerializer.Serialize(resposta, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }
}