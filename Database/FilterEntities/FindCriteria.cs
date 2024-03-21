namespace Database.FilterEntities;

/// <summary>
/// Критерий фильтрации
/// </summary>
/// <remarks>CloudWMS-16</remarks>
/// <author>m.shkodin@axelot.ru</author>
public class FindCriteria
{
    /// <summary>
    /// Свойство для фильтрации
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Значение
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Способ сравнения
    /// </summary>
    public Comparator Comparator { get; set; }
}