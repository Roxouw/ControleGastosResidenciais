using System.Net;
using System.Text.Json;

namespace GastosResidenciais.API.Middleware;

/// <summary>
/// Middleware de tratamento global de exceções.
///
/// Centraliza o tratamento de erros em um único lugar, evitando try/catch
/// espalhados nos controllers. Converte exceções conhecidas em respostas
/// HTTP adequadas com mensagens amigáveis para o cliente.
///
/// Mapeamento de exceções:
///   - KeyNotFoundException     → 404 Not Found
///   - InvalidOperationException → 422 Unprocessable Entity (violação de regra de negócio)
///   - Exception (genérica)     → 500 Internal Server Error
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            // Recurso não encontrado (pessoa, categoria ou transação inexistente)
            _logger.LogWarning(ex, "Recurso não encontrado: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            // Violação de regra de negócio (menor de idade, categoria incompatível, etc.)
            _logger.LogWarning(ex, "Violação de regra de negócio: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.UnprocessableEntity, ex.Message);
        }
        catch (Exception ex)
        {
            // Erros inesperados — não expõe detalhes internos ao cliente
            _logger.LogError(ex, "Erro interno não tratado.");
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError,
                "Ocorreu um erro interno. Por favor, tente novamente.");
        }
    }

    /// <summary>
    /// Escreve a resposta de erro em formato JSON padronizado.
    /// </summary>
    private static async Task WriteErrorResponse(
        HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = (int)statusCode;

        var payload = JsonSerializer.Serialize(new
        {
            status  = (int)statusCode,
            error   = statusCode.ToString(),
            message = message
        });

        await context.Response.WriteAsync(payload);
    }
}
