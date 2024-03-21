using System.Linq.Expressions;
using Database.FilterEntities;

namespace Database.Interfaces;

/// <summary>
/// Интерфейс для описания базовой функциональности репозитория
/// </summary>
/// <remark>TTT-1</remark>
/// <author>ilya.falko2013@gmail.com</author>
public interface IRepository<T>
{
    /// <summary>
    /// Получение сущности по UUID
    /// </summary>
    /// <remark>TTT-1</remark>
    /// <author>ilya.falko2013@gmail.com</author>
    Task<T> ReadAsync(Guid uuid);
    
    /// <summary>
    /// Получение сущностей по списку UUID-ов
    /// </summary>
    /// <remark>TTT-1</remark>
    /// <author>ilya.falko2013@gmail.com</author>
    Task<IEnumerable<T>> ReadAsync(IEnumerable<Guid> uuids);

    /// <summary>
    /// Создать сущность
    /// </summary>
    /// <remark>TTT-1</remark>
    /// <author>ilya.falko2013@gmail.com</author>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <remark>TTT-1</remark>
    /// <author>ilya.falko2013@gmail.com</author>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <remark>TTT-1</remark>
    /// <author>ilya.falko2013@gmail.com</author>
    Task DeleteAsync(Guid uuid);

    /// <summary>
    /// Поиск по фильтрам
    /// </summary>
    /// <remark>TTT-1</remark>
    /// <author>ilya.falko2013@gmail.com</author>
    Task<IEnumerable<T>> FindAsync(Filter? filter, string? orderBy, int? offset, int? count, bool? sortDescending);

    Task<T> FirstOrDefault(Expression<Func<T, bool>> expression);
}