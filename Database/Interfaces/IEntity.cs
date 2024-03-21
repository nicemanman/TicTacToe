namespace Database.Interfaces;

/// <summary>
/// Сущность имеющая уникальный идентификатор
/// </summary>
/// <remarks>CloudWMS-71</remarks>
/// <author>v.vorobiev@axelot.ru</author>
public interface IEntity
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    /// <remarks>CloudWMS-71</remarks>
    /// <author>v.vorobiev@axelot.ru</author>
    public Guid UUID { get; set; }
}