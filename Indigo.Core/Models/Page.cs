using System;

namespace Indigo.Core.Models
{
    /// <summary>
    /// Represents infomation about a page, such as Name and Message
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or Sets the unique number that represents this page in the database
        /// </summary>
        public int PageId { get; set; }
        /// <summary>
        /// Gets or Sets the name of this page
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets the message of this page
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or Sets the last time this page was edited
        /// </summary>
        public DateTime LastEdited { get; set; }
    }
}