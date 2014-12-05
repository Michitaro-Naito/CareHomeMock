using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    public class License
    {
        [Key]
        public int LicenseId { get; set; }

        /// <summary>
        /// Name of License like "医師".
        /// </summary>
        public string Name { get; set; }
    }
}