using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    public class CareManagerIndexVM
    {
        public int CareHomeId { get; set; }
        public List<CareHomeMock.Models.CareManager> CareManagers { get; set; }
    }
}