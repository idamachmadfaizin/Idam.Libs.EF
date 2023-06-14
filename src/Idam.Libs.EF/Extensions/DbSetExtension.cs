using Idam.Libs.EF.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Extensions;
public static class DbSetExtension
{
    /// <summary>
    /// DbSet extension to restore deleted data
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="dbSet"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static TEntity Restore<TEntity>(this DbSet<TEntity> dbSet, TEntity entity)
        where TEntity : class, ISoftDeleteBase
    {
        if (dbSet is null)
        {
            throw new ArgumentNullException(nameof(dbSet));
        }
        if (entity is not ISoftDeleteBase)
        {
            throw new ArgumentException($"{nameof(entity)} must be ISoftDelete or ISoftDeleteUnix");
        }

        if (entity is ISoftDelete softDelete)
        {
            softDelete.DeletedAt = null;
        }
        else if (entity is ISoftDeleteUnix softDeleteUnix)
        {
            softDeleteUnix.DeletedAt = null;
        }

        return entity;
    }
}
