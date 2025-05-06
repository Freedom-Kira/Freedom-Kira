
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleConvertJson.NET.Models.Response;


public class ObjectFieldFilter
{
    private readonly object _instance;
    private readonly HashSet<string> _fieldNames;
    private readonly StringComparer _comparer;

    public ObjectFieldFilter(object instance, IEnumerable<string> fieldNames, bool ignoreCase = true)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        ArgumentNullException.ThrowIfNull(fieldNames);

        _comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        _fieldNames = new HashSet<string>(fieldNames, _comparer);
    }

    public async Task<Dictionary<string, object?>> FilterAsync()
    {
        return await Task.Run(() =>
        {
            var properties = _instance
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var result = new Dictionary<string, object?>(_comparer);

            foreach (var prop in properties)
            {
                if (!_fieldNames.Contains(prop.Name)) continue;

                try
                {
                    result[prop.Name] = prop.GetValue(_instance);
                }
                catch
                {
                    result[prop.Name] = null;
                }
            }

            return result;
        });
    }

    public async Task<string> ToJsonAsync(Formatting formatting = Formatting.None)
    {
        var filtered = await FilterAsync();
        return JsonConvert.SerializeObject(filtered, formatting);
    }
}




//public class ObjectFieldFilter
//{
//    private readonly object instance;
//    private readonly HashSet<string> fieldNames;
//    private readonly StringComparer comparer;

//    public ObjectFieldFilter(object instance, IEnumerable<string> fieldNames, bool ignoreCase = true)
//    {
//        this.instance = instance ?? throw new ArgumentNullException(nameof(instance));

//        ArgumentNullException.ThrowIfNull(fieldNames);

//        comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
//        this.fieldNames = new HashSet<string>(fieldNames, comparer);
//    }

//    public Dictionary<string, object?> Filter()
//    {
//        var properties = instance
//            .GetType()
//            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

//        var result = new Dictionary<string, object?>(comparer);

//        foreach (var prop in properties)
//        {
//            if (fieldNames.Contains(prop.Name))
//            {
//                try
//                {
//                    result[prop.Name] = prop.GetValue(instance);
//                }
//                catch
//                {
//                    result[prop.Name] = null;
//                }
//            }
//        }

//        return result;
//    }

//public IEnumerable ToJson(Formatting formatting = Formatting.None)
//{
//    return Filter().AsEnumerable() ?? [];

//    //return filtered
//    //return JsonConvert.SerializeObject(filtered, formatting);
//}
//}


public static class ObjectFilterExtensions
{
    public static Dictionary<string, object?> FilterFields(
        this object instance,
        IEnumerable<string> fieldNames,
        bool ignoreCase = true)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));
        if (fieldNames == null)
            throw new ArgumentNullException(nameof(fieldNames));

        var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        var desiredFields = new HashSet<string>(fieldNames, comparer);

        var properties = instance
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var result = new Dictionary<string, object?>();
        foreach (var prop in properties)
        {
            if (desiredFields.Contains(prop.Name))
            {
                try
                {
                    result[prop.Name] = prop.GetValue(instance);
                }
                catch
                {
                    result[prop.Name] = null; // ou lançar exceção, dependendo do caso
                }
            }
        }

        return result;
    }

    public static string FilterFieldsAsJson(
        this object instance,
        IEnumerable<string> fieldNames,
        bool ignoreCase = true,
        Formatting formatting = Formatting.None)
    {
        var filtered = instance.FilterFields(fieldNames, ignoreCase);
        return JsonConvert.SerializeObject(filtered, formatting);
    }
}
