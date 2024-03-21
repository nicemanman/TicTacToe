using System.Linq.Expressions;

namespace Database.Utils;

/// <summary>
/// Вспомогательный класс для связывания нескольких Expression-ов
/// </summary>
/// <remarks>CloudWMS-16</remarks>
/// <author>m.shkodin@axelot.ru</author>
public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>() { return f => true; }
    public static Expression<Func<T, bool>> False<T>() { return f => false; }

    /// <summary>
    /// Объединение по условию "Или"
    /// </summary>
    /// <remarks>CloudWMS-16</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }

    /// <summary>
    /// Объединение по условию "И"
    /// </summary>
    /// <remarks>CloudWMS-16</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    }
}