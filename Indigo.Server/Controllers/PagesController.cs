using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Indigo.Core.Models;
using Indigo.Server.Models;

namespace Indigo.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Pages")]
    public class PagesController : Controller
    {
        private readonly IndigoContext _context;

        public PagesController(IndigoContext context)
        {
            _context = context;
        }

        // GET: api/Pages
        [HttpGet]
        public IEnumerable<Page> GetPage()
        {
            return _context.Pages;
        }

        // GET: api/Pages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var page = await _context.Pages.SingleOrDefaultAsync(m => m.PageId == id);

            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }

        // PUT: api/Pages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPage([FromRoute] int id, [FromBody] Page page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != page.PageId)
            {
                return BadRequest();
            }

            _context.Entry(page).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(id))
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

        // POST: api/Pages
        [HttpPost]
        public async Task<IActionResult> PostPage([FromBody] Page page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pages.Add(page);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPage", new { id = page.PageId }, page);
        }

        // DELETE: api/Pages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var page = await _context.Pages.SingleOrDefaultAsync(m => m.PageId == id);
            if (page == null)
            {
                return NotFound();
            }

            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();

            return Ok(page);
        }

        private bool PageExists(int id)
        {
            return _context.Pages.Any(e => e.PageId == id);
        }
    }
}