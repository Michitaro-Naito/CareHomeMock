using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Cached Cluster for Map Search features.
    /// </summary>
    public class Cluster
    {
        [Key]
        public int ClusterId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public virtual ICollection<CareHome> CareHomes { get; set; }



        public Cluster()
        {
            CareHomes = new List<CareHome>();
        }
    }
}