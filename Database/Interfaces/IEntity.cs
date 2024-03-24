namespace Database.Interfaces;

/// <summary>
/// Сущность базы данных с идентификатором
/// </summary>
public interface IEntity
{
    public Guid UUID { get; set; }
}