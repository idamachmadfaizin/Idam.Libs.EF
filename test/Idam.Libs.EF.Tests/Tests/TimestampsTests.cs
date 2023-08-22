using Idam.Libs.EF.Tests.Ekstensions;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Tests.Tests;
public class TimestampsTests : BaseTest
{
    [Fact]
    public async Task Should_SetCreatedAt_OnDateTimeCreate()
    {
        Entities.Boo data = this._booFaker.Generate();
        await this._context.Boos.AddAsync(data);
        await this._context.SaveChangesAsync();

        Entities.Boo? dataFromDb = await this._context.Boos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(this.utcMinValue.ToString("O"), dataFromDb.CreatedAt.ToString("O"));
        Assert.DoesNotMatch(this.utcMinValue.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_UpdateUpdatedAt_OnDateTimeUpdate()
    {
        Entities.Boo data = await AddAsync(this._booFaker.Generate());

        DateTime oldUpdatedAt = data.UpdatedAt;

        data.Name = this._booFaker.Generate().Name;

        this._context.Boos.Update(data);
        var updated = await this._context.SaveChangesAsync();

        Assert.True(updated > 0);

        Entities.Boo? dataFromDb = await this._context.Boos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(oldUpdatedAt.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_SetCreatedAt_OnUnixCreate()
    {
        Entities.Foo data = await AddAsync(this._fooFaker.Generate());

        Entities.Foo? dataFromDb = await this._context.Foos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(this.utcMinValue.ToUnixTimeMilliseconds().ToString(), dataFromDb.CreatedAt.ToString());
        Assert.DoesNotMatch(this.utcMinValue.ToUnixTimeMilliseconds().ToString(), dataFromDb.UpdatedAt.ToString());
    }

    [Fact]
    public async Task Should_UpdateUpdatedAt_OnUnixUpdate()
    {
        Entities.Foo data = await AddAsync(this._fooFaker.Generate());

        await Task.Delay(5);

        var oldUpdatedAt = data.UpdatedAt;

        data.Name = this._fooFaker.Generate().Name;

        this._context.Foos.Update(data);
        var updated = await this._context.SaveChangesAsync();

        Assert.True(updated > 0);

        Entities.Foo? dataFromDb = await this._context.Foos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(oldUpdatedAt.ToString(), dataFromDb.UpdatedAt.ToString());
    }

    [Fact]
    public async Task Should_Throw_When_TimeStampsField_NotMatch()
    {
        Entities.Aoo data = this._aooFaker.Generate();
        await this._context.Aoos.AddAsync(data);

        await Assert.ThrowsAsync<InvalidCastException>(() => this._context.SaveChangesAsync());
    }

    [Fact]
    public async Task Should_SetCreatedAtField_OnCustomTimeStamps()
    {
        Entities.Boo data = this._booFaker.Generate();
        await this._context.Boos.AddAsync(data);
        await this._context.SaveChangesAsync();

        Entities.Boo? dataFromDb = await this._context.Boos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(this.utcMinValue.ToString("O"), dataFromDb.CreatedAt.ToString("O"));
        Assert.DoesNotMatch(this.utcMinValue.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_UpdateUpdatedAtField_OnCustomTimeStamps()
    {
        Entities.Boo data = await AddAsync(this._booFaker.Generate());

        DateTime oldUpdatedAt = data.UpdatedAt;

        data.Name = this._booFaker.Generate().Name;

        this._context.Boos.Update(data);
        var updated = await this._context.SaveChangesAsync();

        Assert.True(updated > 0);

        Entities.Boo? dataFromDb = await this._context.Boos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(oldUpdatedAt.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

}
