using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models.Sql
{
    /// <summary>
    /// Cached Cluster for Map Search features.
    /// </summary>
    public class Cluster
    {
        public double X { get; set; }
        public double Y { get; set; }
        public virtual ICollection<CareHome> CareHomes { get; set; }



        public Cluster()
        {
            CareHomes = new List<CareHome>();
        }
    }
}