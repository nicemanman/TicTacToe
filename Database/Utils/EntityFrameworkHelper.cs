using System.Linq.Expressions;
using System.Reflection;
using Database.Extensions;
using Database.FilterEntities;
using Microsoft.EntityFrameworkCore;

namespace Database.Utils;

/// <summary>
/// Хэлпер для работы с EF
/// </summary>
/// <remarks>CloudWMS-16</remarks>
/// <author>m.shkodin@axelot.ru</author>
public static class EntityFrameworkHelper
{
    private const string _propertyDelimeter = ".";

    /// <summary>
    /// Поиск по фильтрам
    /// </summary>
    /// <remarks>CloudWMS-16</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    public static async Task<IEnumerable<TEntity>> Find<TEntity>(IQueryable<TEntity> query, Filter? filter = null, string? orderBy = null, int? offset = null, int? count = null, bool? sortDescending = null) 
        where TEntity : class
    {
        List<string> alreadyIncluded = new List<string>();
        if (filter?.Criteria?.Any() == true)
        {
            Expression<Func<TEntity, bool>> predicate = PredicateBuilder.True<TEntity>();

            foreach (FindCriteria criteria in filter.Criteria)
            {
                AddIncludedEntities(query, alreadyIncluded, criteria.Field);

                predicate = predicate.And(BuildPredicate<TEntity>(criteria));
            }

            query = query.Where(predicate);
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            bool descending = sortDescending ?? false;
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        if (offset.HasValue)
        {
            query = query.Skip(offset.Value);
        }

        if (count.HasValue)
        {
            query = query.Take(count.Value);
        }

        return await query.ToListAsync();
    }

    private static void AddIncludedEntities<TEntity>(IQueryable<TEntity> query, List<string> alreadyIncluded, string field) where TEntity : class
    {
        if (field.Contains(_propertyDelimeter))
        {
            int indexOfLastDot = field.LastIndexOf(_propertyDelimeter);
            string toInclude = field.Substring(0, indexOfLastDot);
            if (!alreadyIncluded.Any(i => i == toInclude))
            {
                query.Include(toInclude);
                alreadyIncluded.Add(toInclude);
            }
        }
    }

    /// <summary>
    /// Построение Expression-а по фильтру
    /// <summary>
    /// <remarks>CloudWMS-16</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    private static Expression<Func<TEntity, bool>> BuildPredicate<TEntity>(FindCriteria criteria)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TEntity));

        MemberExpression? property = null;
        if (criteria.Field.Contains(_propertyDelimeter))
        {
            foreach (var nestedProperty in criteria.Field.Split('.'))
            {
                property = property == null ? Expression.Property(parameter, nestedProperty) : Expression.Property(property, nestedProperty);
            }
        }
        else
        {
            property = Expression.Property(parameter, criteria.Field);
        }
        ConstantExpression value = Expression.Constant(criteria.Value, property.Type);

        switch (criteria.Comparator)
        {
            case Comparator.Equals:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(property, value), parameter);
            case Comparator.GreaterThan:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThan(property, value), parameter);
            case Comparator.GreaterThanOrEquals:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThanOrEqual(property, value), parameter);
            case Comparator.LessThan:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.LessThan(property, value), parameter);
            case Comparator.LessThanOrEquals:
                return Expression.Lambda<Func<TEntity, bool>>(Expression.LessThanOrEqual(property, value), parameter);
            case Comparator.Like:
                MethodInfo? method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                MethodCallExpression? contains = Expression.Call(property, method, value);
                return Expression.Lambda<Func<TEntity, bool>>(contains, parameter);
            default:
                throw new InvalidOperationException($"Unsupported comparator '{criteria.Comparator}'.");
        }
    }
}