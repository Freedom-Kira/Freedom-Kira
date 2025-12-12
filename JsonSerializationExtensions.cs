using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace api_snake_case.Extensions;

public static class JsonSerializationExtensions
{
    private static readonly SnakeCaseNamingStrategy snakeCaseNamingStrategy
        = new SnakeCaseNamingStrategy
        {
            ProcessDictionaryKeys = true,
            OverrideSpecifiedNames = false,
        };

    private static readonly JsonSerializerSettings _snakeCaseSettings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = snakeCaseNamingStrategy
        }
        //Formatting = Formatting.Indented

    };

    public static string ToSnakeCase<T>(this T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(paramName: nameof(instance));
        }

        return JsonConvert.SerializeObject(instance, _snakeCaseSettings);
    }

    public static string ToSnakeCase(this string @string)
    {
        if (@string == null)
        {
            throw new ArgumentNullException(paramName: nameof(@string));
        }

        return snakeCaseNamingStrategy.GetPropertyName(@string, false);
    }
}

public static class JsonSerializationExtensionsText
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return string.Concat(
                name.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + char.ToLower(c) : char.ToLower(c).ToString())
            );
        }
    }

    public static readonly JsonSerializerOptions DefaultOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower // ou null para PascalCase
    };

    public static string ToJson(this object obj, JsonSerializerOptions? options = null)
        => System.Text.Json.JsonSerializer.Serialize(obj, options);

    public static T? FromJson<T>(this string json, JsonSerializerOptions? options = null)
        => System.Text.Json.JsonSerializer.Deserialize<T>(json, options);

    public static string ToSnakeCase(this string value) =>
        new SnakeCaseNamingPolicy().ConvertName(value ?? throw new ArgumentNullException(nameof(value)));

}