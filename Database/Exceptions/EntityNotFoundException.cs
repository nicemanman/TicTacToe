namespace Database.Exceptions;

/// <summary>
/// Ошибка отсутствия сущности в бд
/// </summary>
/// <remarks>CloudWMS-34</remarks>
/// <author>v.vorobiev@axelot.ru</author>
public class EntityNotFoundException : Exception
{
    private const string _exceptionMessage = "Entity of type: {0} with UUID: {1} was not found " +
                                             "in the database when executing operation: {2}";
    public EntityNotFoundException(string entityId, string entityType, string operationType) : 
        base(String.Format(_exceptionMessage, entityType, entityId, operationType))
    {
    }
}