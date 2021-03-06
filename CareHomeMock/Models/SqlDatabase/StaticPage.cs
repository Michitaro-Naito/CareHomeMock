﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [AllowHtml]
        public string Html { get; set; }

        /// <summary>
        /// If true, this StaticPage is not listed at Information, Layout.
        /// </summary>
        [Display(Name="一覧に表示しない")]
        public bool NotListed { get; set; }



        public StaticPage()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
    }
}