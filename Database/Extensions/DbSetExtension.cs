using Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Extensions;

public static class DbSetExtension
{
    public static bool Exists<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class, IEntity
    {
        return dbSet.Local.Any(e => e.UUID.Equals(entity.UUID));
    }

    public static void Detach<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity: class, IEntity
    {
        var local = dbSet
            .Local
            .FirstOrDefault(entry => entry.UUID.Equals(entity.UUID));
        
        if (local != null)
        {
            dbSet.Entry(local).State = EntityState.Detached;
        }
    }
}