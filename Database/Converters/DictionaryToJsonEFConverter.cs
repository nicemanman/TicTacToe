using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Database.Converters;

/// <summary>
/// Конвертер словаря в строку при записи/чтении в бд через entity framework
/// </summary>
/// <typeparam name="TKey">не null ключ словаря</typeparam>
/// <typeparam name="TValue">значение словаря</typeparam>
/// <remarks>TTT-1</remarks>
/// <author>ilya.falko2013@gmail.com</author>
public class DictionaryToJsonEFConverter<TKey, TValue> : ValueConverter<Dictionary<TKey, TValue>?, string> 
    where TKey : notnull
{
    public DictionaryToJsonEFConverter() : base(
        d => Serialize(d),
        s => Deserialize(s))
    { }

    private static string Serialize(Dictionary<TKey, TValue>? dictionary)
    {
        return JsonConvert.SerializeObject(dictionary);
    }

    private static Dictionary<TKey, TValue>? Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(json);
    }
}