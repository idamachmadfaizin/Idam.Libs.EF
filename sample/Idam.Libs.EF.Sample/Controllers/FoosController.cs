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
        if (_context.Foos == null)
        {
            return NotFound();
        }
        return await _context.Foos.ToListAsync();
    }

    // GET: api/Foos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Foo>> GetFoo(Guid id)
    {
        if (_context.Foos == null)
        {
            return NotFound();
        }
        var foo = await _context.Foos.FindAsync(id);

        if (foo == null)
        {
            return NotFound();
        }

        return foo;
    }

    // PUT: api/Foos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFoo(Guid id, FooUpdateDto foo)
    {
        if (id != foo.Id)
        {
            return BadRequest();
        }

        _context.Entry(foo).State = EntityState.Modified;

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
        if (_context.Foos == null)
        {
            return Problem("Entity set 'MyDbContext.Foos'  is null.");
        }

        var foo = new Foo(fooDto);

        _context.Foos.Add(foo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFoo), new { id = foo.Id }, foo);
    }

    // DELETE: api/Foos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFoo(Guid id)
    {
        if (_context.Foos == null)
        {
            return NotFound();
        }
        var foo = await _context.Foos.FindAsync(id);
        if (foo == null)
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
        if (_context.Foos == null)
        {
            return NotFound();
        }
        return await _context.Foos
            .IgnoreQueryFilters()
            .Where(w => w.DeletedAt != null)
            .ToListAsync();
    }

    // GET: api/Foos/deleted/5
    [HttpGet("deleted/{id}")]
    public async Task<ActionResult<Foo>> GetDeletedFoo(Guid id)
    {
        if (_context.Foos == null)
        {
            return NotFound();
        }
        var foo = await _context.Foos
            .IgnoreQueryFilters()
            .Where(w => w.Id == id)
            .Where(w => w.DeletedAt != null)
            .FirstOrDefaultAsync();

        if (foo == null)
        {
            return NotFound();
        }

        return foo;
    }

    // PUT: api/Foos/restore/5
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> RestoreFoo(Guid id)
    {
        var entity = await _context.Foos
            .IgnoreQueryFilters()
            .Where(w => w.Id == id)
            .Where(w => w.DeletedAt != null)
            .FirstOrDefaultAsync();
        if (entity == null)
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
