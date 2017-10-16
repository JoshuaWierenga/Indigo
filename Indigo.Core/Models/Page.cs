using System;

namespace Indigo.Core
{
    public class Page
    {
        public int PageId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime LastEdited { get; set; }
    }
}