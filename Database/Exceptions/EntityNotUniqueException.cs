namespace Database.Exceptions;

/// <summary>
/// Сущность имеет не уникальное имя или путь
/// </summary>
/// <remark>CloudWMS-127</remark>
/// <author>i.dinikaev@axelot.ru</author>
public class EntityNotUniqueException : Exception
{
    public EntityNotUniqueException(string message = null) : base(message)
    {
            
    }
}