using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Indigo.Server.Models;
using System.Threading.Tasks;
using Indigo.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using Markdig;

namespace Indigo.Server.Controllers
{
    /// <summary>
    /// Handles all connections directed at /home
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Connection to Database
        /// </summary>
        private readonly IndigoContext _context;

        /// <summary>
        /// Stores reference to Database
        /// </summary>
        /// <param name="context">Reference to Database</param>
        public HomeController(IndigoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles all connections directed at /home/index/{pagename}
        /// Searches database for page matching name and displays it if found otherwises displays home page
        /// </summary>
        /// <param name="pagename">Name of page to display</param>
        /// <returns>View containing page</returns>
         // home/index/5
        public async Task<IActionResult> Index(string pagename)
        {
            //If pagename is passed in then searches database for page otherwises requests home page
            Page foundPage = pagename != null ? await GetPage(pagename) : await GetPage("home");

            //converts page message or blank string if it is null to markdown
            string markdownMessage = Markdown.ToHtml(foundPage.Message ?? "", new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());

            //adds markdown converted message to viewdata to be used by view
            ViewData.Add("Markdown", markdownMessage);

            //returns a view containing the page
            return View(foundPage);
        }

        /// <summary>
        /// Handles post connections directed at /home/save with page in request body
        /// Attempts to save page to database if it is valid
        /// </summary>
        /// <param name="page">Page to save to database</param>
        /// <returns>http status 200</returns>
        // POST: home/save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([Bind("PageId,Name,Message,LastEdited")] Page page)
        {
            //checks if input is valid
            if (ModelState.IsValid)
            {
                //replace pages last edit time with the current time in utc
                page.LastEdited = DateTime.UtcNow;
                //uses regex to remove all html tags from the string and then removes spaces from the
                //begining and ends of the string
                page.Message = Regex.Replace(page.Message, @"<[^>]+>|&nbsp;", "").Trim();

                //checks if page is new or not
                if (page.PageId == 0)
                {
                    //adds new page to database
                    _context.Pages.Add(page);
                    //saves changes to database
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //attempts to save new copy of page to database, catches if db copy has been edited
                    try
                    {
                        //updates copy of page within database with new copy
                        _context.Pages.Update(page);
                        //saves changes to database
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        //checks if page no longer exists within database
                        if (await _context.Pages.AnyAsync(p => p.PageId == page.PageId))
                        {
                            //returns http status code 404
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                //returns http status 200
                return Ok();

            }
            //returns a view containing the page
            return View(page);
        }

        /// <summary>
        /// Handles all connections directed at /home/error
        /// handles errors
        /// </summary>
        /// <returns>View containing error</returns>
        // home/error
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Checks if page name matches a page within the database, returns it if it does
        /// </summary>
        /// <param name="pagename">name of page to search for</param>
        /// <returns>Page if it exists new page matching name</returns>
        public async Task<Page> GetPage(string pagename)
        {
            //searches database for page with name matching input, creates new page with input as name if it doesn't exist
            Page foundpage = await _context.Pages.SingleOrDefaultAsync(p => p.Name == pagename) ??
                new Page
                {
                    Name = pagename
                };

            //returns either found or new page
            return foundpage;
        }
    }
}