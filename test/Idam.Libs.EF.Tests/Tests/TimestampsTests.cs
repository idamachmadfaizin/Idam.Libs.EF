using Idam.Libs.EF.Extensions;
using Idam.Libs.EF.Tests.Ekstensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Idam.Libs.EF.Tests.Tests;
public class TimestampsTests : BaseTest
{
    [Fact]
    public void When_ConfigureTimeStamps_ShouldUpdatedValue()
    {
        var services = new ServiceCollection();

        var newUseUtcDateTime = true;
        services.ConfigureTimeStamps(options =>
        {
            options.UseUtcDateTime = newUseUtcDateTime;
        });

        Assert.True(DbContextExtensions.TimeStampsOptions.UseUtcDateTime == newUseUtcDateTime);
    }

    [Fact]
    public async Task DateTimeCreate()
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
    public async Task DateTimeUpdate()
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
    public async Task UnixCreate()
    {
        var data = await this.AddAsync(_fooFaker.Generate());

        var dataFromDb = await _context.Foos.FirstOrDefaultAsync(w => w.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.DoesNotMatch(utcMinValue.ToUnixTimeMilliseconds().ToString(), dataFromDb.CreatedAt.ToString());
        Assert.DoesNotMatch(utcMinValue.ToUnixTimeMilliseconds().ToString(), dataFromDb.UpdatedAt.ToString());
    }

    [Fact]
    public async Task UnixUpdate()
    {
        var data = await this.AddAsync(_fooFaker.Generate());

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
