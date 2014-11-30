using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// StaticPage which has been written by Admin like "About us".
    /// </summary>
    public class StaticPage
    {
        public int StaticPageId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public int Order { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
    }
}