using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Represents a file uploaded by Admin.
    /// Raw data will go FileData, BlobStorage.
    /// </summary>
    public class File
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string RowKey { get; set; }
    }
}