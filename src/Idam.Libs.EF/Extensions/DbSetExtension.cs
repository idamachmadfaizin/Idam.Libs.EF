using Idam.Libs.EF.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    /// <summary>
    /// Extension to use FindAsync() from IQueryable
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<TEntity?> FindAsync<TEntity>(this IQueryable<TEntity> source, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        return source.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Extension to use FindAsync() from IQueryable
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<TEntity?> FindAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        return source.FirstOrDefaultAsync(predicate, cancellationToken);
    }
}
