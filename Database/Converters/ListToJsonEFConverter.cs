using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Database.Converters;

/// <summary>
/// Конвертер листа в строку при записи/чтении в бд через entity framework
/// </summary>
/// <typeparam name="T">значение листа</typeparam>
/// <remarks>CloudWMS-51</remarks>
/// <author>i.dinikaev@axelot.ru</author>
public class ListToJsonEFConverter<T> : ValueConverter<List<T>?, string> 
{
    public ListToJsonEFConverter() : base(
        d => Serialize(d),
        s => Deserialize(s))
    { }

    private static string Serialize(List<T>? list)
    {
        return JsonConvert.SerializeObject(list);
    }

    private static List<T>? Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<List<T>>(json);
    }
}