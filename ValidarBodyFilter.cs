using System.Collections.Concurrent;
using System.Reflection;

namespace api_snake_case.Helpers;

public class ValidarBodyFilter<T> : IEndpointFilter where T : class
{
    // Cache estático das propriedades por tipo
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();
    private static readonly PropertyInfo[] _properties = GetRequiredProperties();

    private static PropertyInfo[] GetRequiredProperties()
    {
        return [.. typeof(T).GetProperties().Where(prop => Nullable.GetUnderlyingType(prop.PropertyType) == null)];
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var body = context.Arguments.OfType<T>().FirstOrDefault();
        if (body == null)
            return Results.BadRequest("Body ausente ou inválido.");

        // Usa cache estático - muito mais rápido
        foreach (var prop in _properties)
        {
            if (Nullable.GetUnderlyingType(prop.PropertyType) != null) continue;

            var value = prop.GetValue(body);
            if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
                return Results.BadRequest($"Campo obrigatório '{prop.Name}' não informado.");
        }

        return await next(context);
    }
}
