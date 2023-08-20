using Idam.Libs.EF.Tests.Ekstensions;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Tests.Tests;
public class TimestampsTests : BaseTest
{
    [Fact]
    public async Task Should_SetCreatedAt_OnDateTimeCreate()
    {
        var data = _booFaker.Generate();
        await _context.Boos.AddAsync(data);
        await _context.SaveChangesAsync();

        var dataFromDb = await _context.Boos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(utcMinValue.ToString("O"), dataFromDb.CreatedAt.ToString("O"));
        Assert.DoesNotMatch(utcMinValue.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_UpdateUpdatedAt_OnDateTimeUpdate()
    {
        var data = await this.AddAsync(_booFaker.Generate());

        var oldUpdatedAt = data.UpdatedAt;

        data.Name = _booFaker.Generate().Name;

        _context.Boos.Update(data);
        var updated = await _context.SaveChangesAsync();

        Assert.True(updated > 0);

        var dataFromDb = await _context.Boos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(oldUpdatedAt.ToString("O"), dataFromDb.UpdatedAt.ToString("O"));
    }

    [Fact]
    public async Task Should_SetCreatedAt_OnUnixCreate()
    {
        var data = await this.AddAsync(_fooFaker.Generate());

        var dataFromDb = await _context.Foos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(utcMinValue.ToUnixTimeMilliseconds().ToString(), dataFromDb.CreatedAt.ToString());
        Assert.DoesNotMatch(utcMinValue.ToUnixTimeMilliseconds().ToString(), dataFromDb.UpdatedAt.ToString());
    }

    [Fact]
    public async Task Should_UpdateUpdatedAt_OnUnixUpdate()
    {
        var data = await this.AddAsync(_fooFaker.Generate());

        await Task.Delay(5);

        var oldUpdatedAt = data.UpdatedAt;

        data.Name = _fooFaker.Generate().Name;

        _context.Foos.Update(data);
        var updated = await _context.SaveChangesAsync();

        Assert.True(updated > 0);

        var dataFromDb = await _context.Foos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(oldUpdatedAt.ToString(), dataFromDb.UpdatedAt.ToString());
    }
}
