using Idam.Libs.EF.Extensions;
using Idam.Libs.EF.Sample.Context;
using Idam.Libs.EF.Sample.Models.Dto;
using Idam.Libs.EF.Sample.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Idam.Libs.EF.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoosController : ControllerBase
    {
        private readonly MyDbContext _context;

        public BoosController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Boos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Boo>>> GetBoos()
        {
            if (_context.Boos is null)
            {
                return NotFound();
            }
            return await _context.Boos.ToListAsync();
        }

        // GET: api/Boos/289c9eaa-3f35-4462-064a-08db6654a8e7
        [HttpGet("{id}")]
        public async Task<ActionResult<Boo>> GetBoo(Guid id)
        {
            if (_context.Boos is null)
            {
                return NotFound();
            }
            var boo = await _context.Boos.FindAsync(id);

            if (boo is null)
            {
                return NotFound();
            }

            return boo;
        }

        // PUT: api/Boos/289c9eaa-3f35-4462-064a-08db6654a8e7
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoo(Guid id, BooUpdateDto booDto)
        {
            if (id != booDto.Id)
            {
                return BadRequest();
            }

            _context.Entry(booDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooExists(id))
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

        // POST: api/Boos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Boo>> PostBoo(BooCreateDto booDto)
        {
            if (_context.Boos is null)
            {
                return Problem("Entity set 'MyDbContext.Boos'  is null.");
            }

            var boo = new Boo(booDto);

            _context.Boos.Add(boo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoo", new { id = boo.Id }, boo);
        }

        // DELETE: api/Boos/289c9eaa-3f35-4462-064a-08db6654a8e7
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoo(Guid id)
        {
            if (_context.Boos is null)
            {
                return NotFound();
            }

            var boo = await _context.Boos.IgnoreQueryFilters()
                .FirstOrDefaultAsync(b => b.Id.Equals(id));

            if (boo is null)
            {
                return NotFound();
            }

            _context.Boos.Remove(boo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Boos/289c9eaa-3f35-4462-064a-08db6654a8e7/force
        [HttpDelete("{id}/force")]
        public async Task<IActionResult> ForceDeleteBoo(Guid id)
        {
            if (_context.Boos is null)
            {
                return NotFound();
            }

            var boo = await _context.Boos.IgnoreQueryFilters()
                .FirstOrDefaultAsync(b => b.Id.Equals(id));

            if (boo is null)
            {
                return NotFound();
            }

            _context.Boos.ForceRemove(boo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Boos/deleted
        [HttpGet("deleted")]
        public async Task<ActionResult<IEnumerable<Boo>>> GetDeletedBoos()
        {
            if (_context.Boos is null)
            {
                return NotFound();
            }
            return await _context.Boos
                .IgnoreQueryFilters()
                .Where(w => w.DeletedAt != null)
                .ToListAsync();
        }

        // GET: api/Boos/deleted/289c9eaa-3f35-4462-064a-08db6654a8e7
        [HttpGet("deleted/{id}")]
        public async Task<ActionResult<Boo>> GetDeletedBoo(Guid id)
        {
            if (_context.Boos is null)
            {
                return NotFound();
            }
            var foo = await _context.Boos
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

        // PUT: api/Boos/restore/289c9eaa-3f35-4462-064a-08db6654a8e7
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreBoo(Guid id)
        {
            var entity = await _context.Boos
                .IgnoreQueryFilters()
                .Where(w => w.Id == id)
                .Where(w => w.DeletedAt != null)
                .FirstOrDefaultAsync();
            if (entity is null)
            {
                return NotFound();
            }

            _context.Boos.Restore(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BooExists(Guid id)
        {
            return (_context.Boos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
