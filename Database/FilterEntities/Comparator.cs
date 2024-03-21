namespace Database.FilterEntities;

/// <summary>
/// Список значений для фильтрации
/// </summary>
/// <remarks>CloudWMS-16</remarks>
/// <author>m.shkodin@axelot.ru</author>
public enum Comparator
{
    Equals,
    GreaterThan,
    GreaterThanOrEquals,
    LessThan,
    LessThanOrEquals,
    Like
}