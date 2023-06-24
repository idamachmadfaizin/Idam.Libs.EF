using Idam.Libs.EF.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Tests.Tests;
public class SoftDeleteTests : BaseTest
{
    [Fact]
    public async Task DateTimeDelete()
    {
        var data = await this.AddAsync(_booFaker.Generate());
        data = await this.DeleteAsync(data);

        var dataFromDb = await _context.Boos.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.NotNull(dataFromDb.DeletedAt);
    }

    [Fact]
    public async Task DateTimeSoftDeleteFilter()
    {
        var data = await this.AddAsync(_booFaker.Generate());
        data = await this.DeleteAsync(data);

        var dataFromDb = await _context.Boos.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task DateTimeRestore()
    {
        var data = await this.AddAsync(_booFaker.Generate());
        data = await this.DeleteAsync(data);

        var dataFromDb = await _context.Boos.IgnoreQueryFilters()
            .Where(w => w.Id.Equals(data.Id))
            .Where(w => w.DeletedAt.HasValue)
            .FirstOrDefaultAsync();

        Assert.NotNull(dataFromDb);
        Assert.NotNull(dataFromDb.DeletedAt);

        _context.Boos.Restore(dataFromDb);
        await _context.SaveChangesAsync();

        dataFromDb = await _context.Boos.FirstOrDefaultAsync(w => w.Id.Equals(dataFromDb.Id));

        Assert.NotNull(dataFromDb);
        Assert.Null(dataFromDb.DeletedAt);
    }

    [Fact]
    public async Task DateTimePemanentDelete()
    {
        var data = await this.AddAsync(_booFaker.Generate());
        data = await this.DeleteAsync(data);
        data = await this.DeleteAsync(data);

        var dataFromDb = await _context.Boos.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));
        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task UnixDelete()
    {
        var data = await this.AddAsync(_fooFaker.Generate());
        data = await this.DeleteAsync(data);

        var dataFromDb = await _context.Foos.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.NotNull(dataFromDb);
        Assert.NotNull(dataFromDb.DeletedAt);
    }

    [Fact]
    public async Task UnixSoftDeleteFilter()
    {
        var data = await this.AddAsync(_fooFaker.Generate());
        data = await this.DeleteAsync(data);

        var dataFromDb = await _context.Foos.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task UnixPemanentDelete()
    {
        var data = await this.AddAsync(this._fooFaker.Generate());
        data = await this.DeleteAsync(data);
        data = await this.DeleteAsync(data);

        var dataFromDb = await _context.Foos.FirstOrDefaultAsync(x => x.Id.Equals(data.Id));

        Assert.Null(dataFromDb);
    }

    [Fact]
    public async Task UnixRestore()
    {
        var data = await this.AddAsync(_fooFaker.Generate());
        data = await this.DeleteAsync(data);

        var dataFromDb = await _context.Foos.IgnoreQueryFilters()
            .Where(w => w.Id.Equals(data.Id))
            .Where(w => w.DeletedAt.HasValue)
            .FirstOrDefaultAsync();

        Assert.NotNull(dataFromDb);
        Assert.NotNull(dataFromDb.DeletedAt);

        _context.Foos.Restore(dataFromDb);
        await _context.SaveChangesAsync();

        dataFromDb = await _context.Foos.FirstOrDefaultAsync(w => w.Id.Equals(dataFromDb.Id));

        Assert.NotNull(dataFromDb);
        Assert.Null(dataFromDb.DeletedAt);
    }
}
