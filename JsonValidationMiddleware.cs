using System.Text.Json;

namespace api_snake_case.Helpers;

public class JsonValidationMiddleware(RequestDelegate next, ILogger<JsonValidationMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<JsonValidationMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex) when (ex.InnerException is JsonException)
        {
            _logger.LogWarning("Erro de validação JSON: {Message}", ex.Message);

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                error = "Dados inválidos no JSON",
                message = "Verifique se todos os campos obrigatórios estão presentes",
                details = ExtractMissingFields(ex.InnerException.Message)
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }

    private static string ExtractMissingFields(string message)
    {
        // Extrai campos ausentes da mensagem de erro
        if (message.Contains("missing required properties"))
        {
            var start = message.IndexOf("including the following: ") + 26;
            if (start > 25)
            {
                var end = message.IndexOf(" at ", start);
                if (end > start)
                {
                    return message.Substring(start, end - start);
                }
            }
        }
        return "Campos obrigatórios ausentes";
    }
}
