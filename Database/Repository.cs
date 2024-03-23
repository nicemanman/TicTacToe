using System.Linq.Expressions;
using Database.Extensions;
using Database.FilterEntities;
using Database.Interfaces;
using Database.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database;

public class Repository<T> : IRepository<T> where T: class, IEntity, new()
{
    protected DbSet<T> _dbSet;

    public Repository(DbSet<T> dbSet)
    {
        _dbSet = dbSet;
    }

    /// <summary>
    /// Создание сущности
    /// </summary>
    /// <remark>CloudWMS-174</remark>
    /// <author>i.falko@axelot.ru</author>
    public virtual async Task<T> CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        return entity;
    }

    /// <summary>
    /// Удаление сущности
    /// </summary>
    /// <remark>CloudWMS-39</remark>
    /// <author>i.falko@axelot.ru</author>
    public virtual async Task DeleteAsync(Guid uuid)
    {
        T entity = new T() { UUID = uuid };
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Поиск сущностей
    /// </summary>
    /// <remark>CloudWMS-39</remark>
    /// <author>i.falko@axelot.ru</author>
    public virtual async Task<IEnumerable<T>> FindAsync(Filter? filter, 
        string? orderBy, int? offset, int? count, bool? sortDescending)
    {
        return await EntityFrameworkHelper.Find(_dbSet, filter, orderBy, offset, count, sortDescending);
    }
    
    /// <summary>
    /// Поиск сущностей
    /// </summary>
    /// <remark>CloudWMS-39</remark>
    /// <author>i.falko@axelot.ru</author>
    public async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    /// <summary>
    /// Получение сущности
    /// </summary>
    /// <remark>CloudWMS-39</remark>
    /// <author>i.falko@axelot.ru</author>
    public virtual async Task<T> GetAsync(Guid uuid)
    {
        T? entity = await _dbSet.FirstOrDefaultAsync(u => u.UUID == uuid);
        return entity;
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        return _dbSet.ToList();
    }
    
    /// <summary>
    /// Получение списка сущностей
    /// </summary>
    /// <remark>CloudWMS-39</remark>
    /// <author>i.falko@axelot.ru</author>
    public virtual async Task<IEnumerable<T>> GetAsync(IEnumerable<Guid> uuids)
    {
        IEnumerable<T> entities = _dbSet.Where(w => uuids.Contains(w.UUID));
        return entities.ToList();
    }

    /// <summary>
    /// Обновление сущности
    /// </summary>
    /// <remark>CloudWMS-39</remark>
    /// <author>i.falko@axelot.ru</author>
    public virtual async Task<T> UpdateAsync(T entity)
    {
        EntityEntry<T> updatedEntity = _dbSet.Update(entity);
        return updatedEntity.Entity;
    }
}