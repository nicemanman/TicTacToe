using System.Linq.Expressions;

namespace Database.Interfaces;

/// <summary>
/// Репозиторий для работы с базой данных
/// </summary>
public interface IRepository<T>
{
    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    Task<T> GetAsync(Guid uuid);
    
    /// <summary>
    /// Получить сущности по идентификаторам
    /// </summary>
    Task<IEnumerable<T>> GetAsync(IEnumerable<Guid> uuids);

    /// <summary>
    /// Получить все сущности
    /// </summary>
    Task<List<T>> GetAllAsync();
    
    /// <summary>
    /// Создать сущность
    /// </summary>
    Task<T> CreateAsync(T entity);
    
    /// <summary>
    /// Обновить сущность
    /// </summary>
    Task<T> UpdateAsync(T entity);
    
    /// <summary>
    /// Удалить сущность
    /// </summary>
    Task DeleteAsync(Guid uuid);

    /// <summary>
    /// Найти первую сущность, подходящую под условие
    /// </summary>
    Task<T> FirstOrDefault(Expression<Func<T, bool>> expression);
}