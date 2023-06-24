using Idam.Libs.EF.Tests.Context;
using Idam.Libs.EF.Tests.Faker;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Tests.Tests;

public abstract class BaseTest
{
    protected readonly TestDbContext _context;

    protected readonly BooFaker _booFaker;
    protected readonly FooFaker _fooFaker;

    protected readonly DateTime utcMinValue;

    public BaseTest()
    {
        _context = new TestDbContext();
        _context.Database.EnsureCreated();

        _booFaker = new BooFaker();
        _fooFaker = new FooFaker();
        utcMinValue = DateTime.MinValue.ToUniversalTime();
    }

    /// <summary>Generic add async</summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    protected async Task<TEntity> AddAsync<TEntity>(TEntity? data)
        where TEntity : class
    {
        Assert.NotNull(data);

        _context.Set<TEntity>().Add(data);
        var created = await this._context.SaveChangesAsync();

        Assert.True(created > 0);
        return data;
    }

    /// <summary>Generic Delete async</summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    protected async Task<TEntity> DeleteAsync<TEntity>(TEntity? data)
        where TEntity : class
    {
        Assert.NotNull(data);

        this._context.Set<TEntity>().Remove(data);
        this._context.Entry(data).State = EntityState.Deleted;
        var removed = await this._context.SaveChangesAsync();

        Assert.True(removed > 0);
        return data;
    }
}