using System.ComponentModel;
using System.Reflection;
using Database.FilterEntities;
using Microsoft.Extensions.Primitives;

namespace Database.Utils;

/// <summary>
/// Хэлпер для парсинга параметров в и строки и формирования фильтра
/// </summary>
/// <remarks>CloudWMS-21</remarks>
/// <author>m.shkodin@axelot.ru</author>
public static class MapFilterHelper
{
    private const string _findFilterDelimeter = "_";
    private const string _propertyDelimeter = ".";
    private const string _invalidPropertPathError = "Invalid property {0} in path {1}";
    private const string _valueCastExceptionError = "Unable to cast value {0} to type {2}";
    private const string _valueCanNotBeNullError = "Value for field {1} can't be null";

    private static Dictionary<string, Comparator> _comparersDictionary = new Dictionary<string, Comparator>()
    {
        {"eq", Comparator.Equals },
        {"gt", Comparator.GreaterThan },
        {"lt", Comparator.LessThan },
        {"goe", Comparator.GreaterThanOrEquals },
        {"loe", Comparator.LessThanOrEquals },
        {"like", Comparator.Like }
    };

    #region StringMapFilter
    /// <summary>
    /// Преобразование набора строковых фильтров в объект Filter
    /// </summary>
    /// <remarks>CloudWMS-21</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    public static Filter MapFilterFromString(IEnumerable<KeyValuePair<string, StringValues>> filters)
    {
        Filter filter = new Filter()
        {
            Criteria = new List<FindCriteria>()
        };

        foreach (KeyValuePair<string, StringValues> urlFilter in filters)
        {
            FindCriteria criteria = GetFindCriteria(urlFilter.Key, urlFilter.Value[0]);

            filter.Criteria.Add(criteria);
        }

        return filter;
    }

    /// <summary>
    /// Получение фильтра поиска по строке и значению
    /// </summary>
    /// <remarks>CloudWMS-21</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    private static FindCriteria GetFindCriteria(string findParam, string? stringValue)
    {
        string[] findParamParts = findParam.Split(_findFilterDelimeter);
        string field = findParamParts[1];
        string comparator = findParamParts[2];

        FindCriteria criteria = new FindCriteria()
        {
            Comparator = _comparersDictionary[comparator],
            Field = field,
            Value = stringValue
        };

        return criteria;
    }

    #endregion StringMapFilter

    /// <summary>
    /// Преобразование набора строковых фильтров в объект Filter
    /// </summary>
    /// <remarks>CloudWMS-21</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    public static Filter MapFilterFromString<T>(IEnumerable<KeyValuePair<string, StringValues>> filters) where T : class
    {
        Filter filter = new Filter()
        {
            Criteria = new List<FindCriteria>()
        };

        foreach(KeyValuePair<string, StringValues> urlFilter in filters)
        {
            FindCriteria criteria = GetFindCriteria<T>(urlFilter.Key, urlFilter.Value[0]);

            filter.Criteria.Add(criteria);
        }

        return filter;
    }

    /// <summary>
    /// Получение фильтра поиска по строке и значению
    /// </summary>
    /// <remarks>CloudWMS-21</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    private static FindCriteria GetFindCriteria<T>(string findParam, string? stringValue) where T : class
    {
        string[] findParamParts = findParam.Split(_findFilterDelimeter);
        string field = findParamParts[1];
        string comparator = findParamParts[2];

        FindCriteria criteria = new FindCriteria()
        {
            Comparator = _comparersDictionary[comparator],
            Field = field,
            Value = GetFindCriteriaValue<T>(field, stringValue)
        };

        return criteria;
    }

    /// <summary>
    /// Каст строкого значения в тип, который описан в сущности
    /// </summary>
    /// <remarks>CloudWMS-21</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    private static object GetFindCriteriaValue<T>(string field, string? stringValue) where T : class
    {
        if (stringValue is null)
            throw new ArgumentException(string.Format(_valueCanNotBeNullError, field));

        Type type = typeof(T);

        if (field.Contains(_propertyDelimeter))
        {
            string[] properties = field.Split(_propertyDelimeter);
            foreach (string propertyName in properties)
            {
                PropertyInfo? propertyInfo = type.GetProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

                if(propertyInfo is null)
                {
                    throw new ArgumentException(string.Format(_invalidPropertPathError, propertyName, field));
                }

                type = propertyInfo.PropertyType;
            }
        }
        else
        {
            PropertyInfo? propertyInfo = type.GetProperties().FirstOrDefault(p => string.Equals(p.Name, field, StringComparison.OrdinalIgnoreCase));

            if (propertyInfo is null)
            {
                throw new ArgumentException(string.Format(_invalidPropertPathError, field, field));
            }

            type = propertyInfo.PropertyType;
        }

        TypeConverter? typeConverter = TypeDescriptor.GetConverter(type);

        object? propValue = typeConverter?.ConvertFromString(stringValue);

        if(propValue is null)
            throw new InvalidCastException(string.Format(_valueCastExceptionError, stringValue, type.FullName));
                

        return propValue;
    }
}