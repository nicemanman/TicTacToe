using System.Linq.Expressions;

namespace Database.Interfaces;

public interface IRepository<T>
{
    Task<T> GetAsync(Guid uuid);
    
    Task<IEnumerable<T>> GetAsync(IEnumerable<Guid> uuids);

    Task<List<T>> GetAllAsync();
    
    Task<T> CreateAsync(T entity);
    
    Task<T> UpdateAsync(T entity);
    
    Task DeleteAsync(Guid uuid);

    Task<T> FirstOrDefault(Expression<Func<T, bool>> expression);
}