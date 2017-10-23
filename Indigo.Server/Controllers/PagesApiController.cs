using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Indigo.Core.Models;
using Indigo.Server.Models;
using System.Text.RegularExpressions;

namespace Indigo.Server.Controllers
{
    /// <summary>
    /// Handles all connections directed at /api
    /// </summary>
    [Produces("application/json")]
    [Route("api")]
    public class PagesApiController : Controller
    {
        /// <summary>
        /// Connection to Database
        /// </summary>
        private readonly IndigoContext _context;

        /// <summary>
        /// Stores reference to Database
        /// </summary>
        /// <param name="context">Reference to Database</param>
        public PagesApiController(IndigoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles get connections directed at /api/{pagename}
        /// Searches database for page matching name and returns it if found otherwises returns home page
        /// </summary>
        /// <param name="pageName">Name of page to return</param>
        /// <returns>http status 200 with page data, gets converted to json before sending</returns>
        // GET: api/5
        [HttpGet("{*pageName}")]
        public async Task<IActionResult> GetPage([FromRoute] string pageName)
        {
            //checks if page name was passed in
            if (!ModelState.IsValid)
            {
                //returns http status 400 and error info
                return BadRequest(ModelState);
            }

            //searches database for page matching name
            var foundPage = await _context.Pages.SingleOrDefaultAsync(p => p.Name == pageName);

            //checks if page exists
            if (foundPage == null)
            {
                //returns http status 404 if page isn't found
                return NotFound();
            }

            //http status 200 with page data
            return Ok(foundPage);
        }

        /// <summary>
        /// Handles put connections directed at /api/{pagename} with page in request body
        /// Attempts to update page in database if it is valid
        /// </summary>
        /// <param name="pageName">Name of page to update</param>
        /// <param name="page">Page to update in database</param>
        /// <returns>http status 204</returns>
        // PUT: api/5
        [HttpPut("{*pageName}")]
        public async Task<IActionResult> PutPage([FromRoute] string pageName, [FromBody] Page page)
        {
            //checks if input is valid
            if (!ModelState.IsValid)
            {
                //returns http status 400 and error info
                return BadRequest(ModelState);
            }

            //makes sure that page object is meant for current page
            if (pageName != page.Name)
            {
                return BadRequest();
            }
            //uses regex to remove all html tags from the string and then removes spaces from the
            //begining and ends of the string
            page.Message = Regex.Replace(page.Message, @"<[^>]+>|&nbsp;", "").Trim();

            //updates copy of page within database with new copy and marks as modified
            _context.Entry(page).State = EntityState.Modified;

            //attempts to save new copy of page to database, catches if db copy has been edited
            try
            {
                //saves changes to database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //checks if page no longer exists within database
                if (!_context.Pages.Any(p => p.Name == page.Name))
                {
                    //returns http status code 404
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //returns http status 204
            return NoContent();
        }

        /// <summary>
        /// Handles post connections directed at /api with page in request body
        /// Attempts to create page in database if it is valid
        /// </summary>
        /// <param name="page">Page to create in database</param>
        /// <returns>http status 201 with url of new page</returns>
        // POST: api
        [HttpPost]
        public async Task<IActionResult> PostPage([FromBody] Page page)
        {
            //checks if input is valid
            if (!ModelState.IsValid)
            {
                //returns http status 400 and error info
                return BadRequest(ModelState);
            }

            //uses regex to remove all html tags from the string and then removes spaces from the
            //begining and ends of the string
            page.Message = Regex.Replace(page.Message, @"<[^>]+>|&nbsp;", "").Trim();

            //adds new page to database
            _context.Pages.Add(page);
            //saves changes to database
            await _context.SaveChangesAsync();

            //returns http status 201 with url of new page
            return CreatedAtAction("GetPage", new { PageName = page.Name }, page);
        }
    }
}