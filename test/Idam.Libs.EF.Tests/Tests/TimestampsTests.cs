using Idam.Libs.EF.Tests.Ekstensions;
using Idam.Libs.EF.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Tests.Tests;
public class TimestampsTests : BaseTest
{
    [Fact]
    public async Task Should_SetCreatedAt_OnDateTimeCreate()
    {
        Boo data = this._booFaker.Generate();
        _ = await this._context.Boos.AddAsync(data);
        _ = await this._context.SaveChangesAsync();

        Boo? dataFromDb = await this._context.Boos
            .FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(this.utcMinValue.ToString("O"), dataFromDb.CreatedAt.ToString("O"));
        Assert.DoesNotMatch(this.utcMinValue.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_UpdateUpdatedAt_OnDateTimeUpdate()
    {
        Boo data = await AddAsync(this._booFaker.Generate());

        DateTime oldUpdatedAt = data.UpdatedAt;

        data.Name = this._booFaker.Generate().Name;

        _ = this._context.Boos.Update(data);
        var updated = await this._context.SaveChangesAsync();

        Assert.True(updated > 0);

        Boo? dataFromDb = await this._context.Boos
            .FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(oldUpdatedAt.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_SetCreatedAt_OnUnixCreate()
    {
        Foo data = await AddAsync(this._fooFaker.Generate());

        Foo? dataFromDb = await this._context.Foos
            .FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(this.utcMinValue.ToUnixTimeMilliseconds().ToString(), dataFromDb.CreatedAt.ToString());
        Assert.DoesNotMatch(this.utcMinValue.ToUnixTimeMilliseconds().ToString(), dataFromDb.UpdatedAt.ToString());
    }

    [Fact]
    public async Task Should_UpdateUpdatedAt_OnUnixUpdate()
    {
        Foo data = await AddAsync(this._fooFaker.Generate());

        await Task.Delay(5);

        var oldUpdatedAt = data.UpdatedAt;

        data.Name = this._fooFaker.Generate().Name;

        _ = this._context.Foos.Update(data);
        var updated = await this._context.SaveChangesAsync();

        Assert.True(updated > 0);

        Foo? dataFromDb = await this._context.Foos
            .FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(oldUpdatedAt.ToString(), dataFromDb.UpdatedAt.ToString());
    }

    [Fact]
    public async Task Should_Throw_When_TimeStampsField_NotMatch()
    {
        Aoo data = this._aooFaker.Generate();
        _ = await this._context.Aoos.AddAsync(data);

        _ = await Assert.ThrowsAsync<InvalidCastException>(() => this._context.SaveChangesAsync());
    }

    [Fact]
    public async Task Should_SetCreatedAtField_OnCustomTimeStamps()
    {
        Boo data = this._booFaker.Generate();
        _ = await this._context.Boos.AddAsync(data);
        _ = await this._context.SaveChangesAsync();

        Boo? dataFromDb = await this._context.Boos
            .FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(this.utcMinValue.ToString("O"), dataFromDb.CreatedAt.ToString("O"));
        Assert.DoesNotMatch(this.utcMinValue.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_UpdateUpdatedAtField_OnCustomTimeStamps()
    {
        Boo data = await AddAsync(this._booFaker.Generate());

        DateTime oldUpdatedAt = data.UpdatedAt;

        data.Name = this._booFaker.Generate().Name;

        _ = this._context.Boos.Update(data);
        var updated = await this._context.SaveChangesAsync();

        Assert.True(updated > 0);

        Boo? dataFromDb = await this._context.Boos
            .FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(oldUpdatedAt.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_Success_When_OnlyUseUpdatedAtFieldOfTimeStamps()
    {
        Eoo data = await AddAsync(this._eooFaker.Generate());

        Eoo? dataFromDb = await this._context.Eoos
            .FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(DateTime.MinValue.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task When_TimeStampsUtc_TheDateTimeKind_Should_Utc()
    {
        Goo data = await AddAsync(this._gooFaker.Generate());

        Goo? dataFromDb = await this._context.Goos
            .FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(DateTime.MinValue.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
        Assert.True(dataFromDb.CreatedAt.Kind.Equals(DateTimeKind.Utc));
        Assert.True(dataFromDb.UpdatedAt.Kind.Equals(DateTimeKind.Utc));
    }
}
