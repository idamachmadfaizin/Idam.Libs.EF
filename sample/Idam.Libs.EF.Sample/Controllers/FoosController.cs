using Idam.Libs.EF.Extensions;
using Idam.Libs.EF.Sample.Context;
using Idam.Libs.EF.Sample.Models.Dto;
using Idam.Libs.EF.Sample.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoosController : ControllerBase
{
    private readonly MyDbContext _context;

    public FoosController(MyDbContext context)
    {
        _context = context;
    }

    // GET: api/Foos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Foo>>> GetFoos()
    {
        if (_context.Foos is null)
        {
            return NotFound();
        }
        return await _context.Foos.ToListAsync();
    }

    // GET: api/Foos/289c9eaa-3f35-4462-064a-08db6654a8e7
    [HttpGet("{id}")]
    public async Task<ActionResult<Foo>> GetFoo(Guid id)
    {
        if (_context.Foos is null)
        {
            return NotFound();
        }
        var foo = await _context.Foos.FindAsync(id);

        if (foo is null)
        {
            return NotFound();
        }

        return foo;
    }

    // PUT: api/Foos/289c9eaa-3f35-4462-064a-08db6654a8e7
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFoo(Guid id, FooUpdateDto fooDto)
    {
        if (id != fooDto.Id)
        {
            return BadRequest();
        }

        _context.Entry(fooDto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FooExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Foos
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Foo>> PostFoo(FooCreateDto fooDto)
    {
        if (_context.Foos is null)
        {
            return Problem("Entity set 'MyDbContext.Foos'  is null.");
        }

        var foo = new Foo(fooDto);

        _context.Foos.Add(foo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFoo), new { id = foo.Id }, foo);
    }

    // DELETE: api/Foos/289c9eaa-3f35-4462-064a-08db6654a8e7
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFoo(Guid id, [FromQuery] bool permanent = false)
    {
        if (_context.Foos is null)
        {
            return NotFound();
        }

        var foo = await _context.Foos.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.Id.Equals(id));

        if (foo is null || (foo.DeletedAt is not null && permanent.Equals(false)))
        {
            return NotFound();
        }

        _context.Foos.Remove(foo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Foos/deleted
    [HttpGet("deleted")]
    public async Task<ActionResult<IEnumerable<Foo>>> GetDeletedFoos()
    {
        if (_context.Foos is null)
        {
            return NotFound();
        }

        return await _context.Foos
            .IgnoreQueryFilters()
            .Where(w => w.DeletedAt != null)
            .ToListAsync();
    }

    // GET: api/Foos/deleted/289c9eaa-3f35-4462-064a-08db6654a8e7
    [HttpGet("deleted/{id}")]
    public async Task<ActionResult<Foo>> GetDeletedFoo(Guid id)
    {
        if (_context.Foos is null)
        {
            return NotFound();
        }

        var foo = await _context.Foos
            .IgnoreQueryFilters()
            .Where(w => w.Id == id)
            .Where(w => w.DeletedAt != null)
            .FirstOrDefaultAsync();

        if (foo is null)
        {
            return NotFound();
        }

        return foo;
    }

    // PUT: api/Foos/restore/289c9eaa-3f35-4462-064a-08db6654a8e7
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> RestoreFoo(Guid id)
    {
        var entity = await _context.Foos
            .IgnoreQueryFilters()
            .Where(w => w.Id == id)
            .Where(w => w.DeletedAt != null)
            .FirstOrDefaultAsync();

        if (entity is null)
        {
            return NotFound();
        }

        _context.Foos.Restore(entity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FooExists(Guid id)
    {
        return (_context.Foos?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
