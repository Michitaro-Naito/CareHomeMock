using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// CareManager hasMany CareManagerLicenses.
    /// License hasMany CareManagerLicenses.
    /// Then, CareManager hasAndBelongsToMany Licenses.
    /// </summary>
    public class CareManagerLicenses
    {
        [Key]
        public int CareManagerLicensesId { get; set; }

        public int CareManagerId { get; set; }

        [ForeignKey("CareManagerId")]
        public CareManager CareManager { get; set; }

        public int LicenseId { get; set; }

        [ForeignKey("LicenseId")]
        public License License { get; set; }
    }
}