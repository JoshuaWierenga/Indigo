using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Indigo.Server.Models;
using System.Threading.Tasks;
using Indigo.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Indigo.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly IndigoContext _context;

        public HomeController(IndigoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string pagename)
        {
            Page foundPage = pagename != null ? await GetPage(pagename) : await GetPage("home");

            return View(foundPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([Bind("PageId,Name,Message,LastEdited")] Page page)
        {
            if (ModelState.IsValid)
            {
                page.LastEdited = DateTime.UtcNow;
                if (page.PageId == 0)
                {
                    _context.Add(page);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        _context.Update(page);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await _context.Pages.AnyAsync(p => p.PageId == page.PageId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }   
                }
                return Ok();

            }
                
            return View(page);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<Page> GetPage(string pagename)
        {
            Page foundpage = await _context.Pages.SingleOrDefaultAsync(p => p.Name == pagename) ??
                new Page
                {
                    Name = pagename
                };

            return foundpage;
        }
    }
}
