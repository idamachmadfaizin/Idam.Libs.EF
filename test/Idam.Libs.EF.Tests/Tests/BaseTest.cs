using Idam.Libs.EF.Tests.Context;
using Idam.Libs.EF.Tests.Faker;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Tests.Tests;

public abstract class BaseTest
{
    protected readonly TestDbContext _context;

    protected readonly AooFaker _aooFaker;
    protected readonly BooFaker _booFaker;
    protected readonly CooFaker _cooFaker;
    protected readonly DooFaker _dooFaker;
    protected readonly EooFaker _eooFaker;
    protected readonly FooFaker _fooFaker;

    protected readonly DateTime utcMinValue;

    public BaseTest()
    {
        this._context = new TestDbContext();
        this._context.Database.EnsureCreated();

        this._aooFaker = new();
        this._booFaker = new();
        this._cooFaker = new();
        this._dooFaker = new();
        this._eooFaker = new();
        this._fooFaker = new();

        this.utcMinValue = DateTime.MinValue.ToUniversalTime();
    }

    /// <summary>Generic add async</summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    protected async Task<TEntity> AddAsync<TEntity>(TEntity? data)
        where TEntity : class
    {
        Assert.NotNull(data);

        this._context.Set<TEntity>().Add(data);
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