using Idam.Libs.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Tests.Tests;
public class SoftDeleteTests : BaseTest
{
    [Fact]
    public async Task Should_SetDeletedAt_OnSoftDelete()
    {
        Entities.Boo data = await AddAsync(this._booFaker.Generate());
        data = await DeleteAsync(data);

        Entities.Boo? dataFromDb = await this._context.Boos.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.NotNull(dataFromDb.DeletedAt);
    }

    [Fact]
    public async Task When_SoftDeleted_ShouldClearDataFromNewList()
    {
        Entities.Boo data = await AddAsync(this._booFaker.Generate());
        data = await DeleteAsync(data);

        Entities.Boo? dataFromDb = await this._context.Boos.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task Should_RestoreSoftDeletedEntity()
    {
        Entities.Boo data = await AddAsync(this._booFaker.Generate());
        data = await DeleteAsync(data);

        Entities.Boo? dataFromDb = await this._context.Boos.IgnoreQueryFilters()
            .Where(w => w.Id.Equals(data.Id))
            .Where(w => w.DeletedAt.HasValue)
            .FirstOrDefaultAsync();

        Assert.NotNull(dataFromDb);
        Assert.NotNull(dataFromDb.DeletedAt);

        this._context.Boos.Restore(dataFromDb);
        await this._context.SaveChangesAsync();

        dataFromDb = await this._context.Boos.FirstOrDefaultAsync(w => w.Id.Equals(dataFromDb.Id));

        Assert.NotNull(dataFromDb);
        Assert.Null(dataFromDb.DeletedAt);
    }

    [Fact]
    public async Task Should_PerformPermanentDelete_AfterSoftDelete()
    {
        Entities.Boo data = await AddAsync(this._booFaker.Generate());
        data = await DeleteAsync(data);
        data = await DeleteAsync(data);

        Entities.Boo? dataFromDb = await this._context.Boos.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));
        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task Should_SetDeletedAt_OnUnixSoftDelete()
    {
        Entities.Foo data = await AddAsync(this._fooFaker.Generate());
        data = await DeleteAsync(data);

        Entities.Foo? dataFromDb = await this._context.Foos.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.NotNull(dataFromDb.DeletedAt);
    }

    [Fact]
    public async Task When_SoftDeleted_ShouldClearUnixDataFromNewList()
    {
        Entities.Foo data = await AddAsync(this._fooFaker.Generate());
        data = await DeleteAsync(data);

        Entities.Foo? dataFromDb = await this._context.Foos.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task Should_PerformPermanentDelete_AfterUnixSoftDelete()
    {
        Entities.Foo data = await AddAsync(this._fooFaker.Generate());
        data = await DeleteAsync(data);
        data = await DeleteAsync(data);

        Entities.Foo? dataFromDb = await this._context.Foos.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task Should_RestoreUnixSoftDeletedEntity()
    {
        Entities.Foo data = await AddAsync(this._fooFaker.Generate());
        data = await DeleteAsync(data);

        Entities.Foo? dataFromDb = await this._context.Foos.IgnoreQueryFilters()
            .Where(w => w.Id.Equals(data.Id))
            .Where(w => w.DeletedAt.HasValue)
            .FirstOrDefaultAsync();

        Assert.NotNull(dataFromDb);
        Assert.NotNull(dataFromDb.DeletedAt);

        this._context.Foos.Restore(dataFromDb);
        await this._context.SaveChangesAsync();

        dataFromDb = await this._context.Foos.FirstOrDefaultAsync(w => w.Id.Equals(dataFromDb.Id));

        Assert.NotNull(dataFromDb);
        Assert.Null(dataFromDb.DeletedAt);
    }

    [Fact]
    public async Task When_ForceDelete_ShouldPerformPermanentDelete()
    {
        Entities.Boo data = await AddAsync(this._booFaker.Generate());
        this._context.Boos.ForceRemove(data);
        await this._context.SaveChangesAsync();

        Entities.Boo? dataFromDb = await this._context.Boos.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task Should_Throw_When_DataTypeOfTimeStampsFieldIsInvalid()
    {
        Entities.Doo data = await AddAsync(this._dooFaker.Generate());

        await Assert.ThrowsAsync<InvalidCastException>(() => DeleteAsync(data));
    }
}
