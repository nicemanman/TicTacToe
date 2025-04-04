using System.Linq.Expressions;
using Database.Interfaces;
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
    
    public virtual async Task<T> CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        return entity;
    }
    
    public virtual Task<T> DeleteAsync(Guid uuid)
    {
        T entity = new T() { UUID = uuid };
        var deletedEntity = _dbSet.Remove(entity);
        return Task.FromResult(deletedEntity.Entity);
    }
    
    public virtual async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    public bool Exists(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Any(expression);
    }

    public virtual async Task<T> GetAsync(Guid uuid)
    {
        T? entity = await _dbSet.FirstOrDefaultAsync(u => u.UUID == uuid);
        return entity;
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        return _dbSet.ToList();
    }
    
    public virtual async Task<IEnumerable<T>> GetAsync(IEnumerable<Guid> uuids)
    {
        IEnumerable<T> entities = _dbSet.Where(w => uuids.Contains(w.UUID));
        return entities.ToList();
    }
    
    public virtual async Task<T> UpdateAsync(T entity)
    {
        EntityEntry<T> updatedEntity = _dbSet.Update(entity);
        return updatedEntity.Entity;
    }
}