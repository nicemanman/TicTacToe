using System.Linq.Expressions;
using System.Reflection;

namespace Database.Extensions;

/// <summary>
/// Методы расширения LINQ
/// <summary>
public static class LinqExtensions
{
    private const string _orderByFunctionName = "OrderBy";
    private const string _orderByDescendingFunctionName = "OrderByDescending";

    /// <summary>
    /// Сортировка по возрастанию для заданного свойства
    /// </summary>
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering)
    {
        MethodCallExpression resultExp = GetOrderMethodeExpression(source, _orderByFunctionName, ordering);
        return source.Provider.CreateQuery<T>(resultExp);
    }

    /// <summary>
    /// Сортировка по убыванию для заданного свойства
    /// </summary>
    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string ordering)
    {
        MethodCallExpression resultExp = GetOrderMethodeExpression(source, _orderByDescendingFunctionName, ordering);
        return source.Provider.CreateQuery<T>(resultExp);
    }

    /// <summary>
    /// Получение Expession-а сортировки по типу
    /// </summary>
    private static MethodCallExpression GetOrderMethodeExpression<T>(IQueryable<T> source, string orderName, string ordering)
    {
        Type type = typeof(T);
        PropertyInfo property = type.GetProperty(ordering);
        ParameterExpression parameter = Expression.Parameter(type, "p");
        MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
        LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);
        MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

        return resultExp;
    }
}