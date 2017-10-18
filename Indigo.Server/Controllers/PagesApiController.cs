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
    [Route("api")]
    public class PagesApiController : Controller
    {
        private readonly IndigoContext _context;

        public PagesApiController(IndigoContext context)
        {
            _context = context;
        }

        // GET: api/Pages/5
        [HttpGet("{*pageName}")]
        public async Task<IActionResult> GetPage([FromRoute] string pageName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foundPage = await _context.Pages.SingleOrDefaultAsync(p => p.Name == pageName);

            if (foundPage == null)
            {
                return NotFound();
            }

            return Ok(foundPage);
        }

        // PUT: api/Pages/5
        [HttpPut("{*pageName}")]
        public async Task<IActionResult> PutPage([FromRoute] string pageName, [FromBody] Page page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pageName != page.Name)
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
                if (!_context.Pages.Any(p => p.Name == page.Name))
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

            return CreatedAtAction("GetPage", new { PageName = page.Name }, page);
        }
    }
}