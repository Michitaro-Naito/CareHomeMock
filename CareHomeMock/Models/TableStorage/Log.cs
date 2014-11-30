using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models.TableStorage
{
    /// <summary>
    /// Represents a Log.
    /// Describes what happened on this website.
    /// </summary>
    public class Log : TableEntity
    {
        // PartitionKey = Kind(Fatal,Admin,CareHome,CareManager,Others)
        // RowKey = Timestamp + GUID, Descending

        public DateTime Created { get; set; }
        public string IpAddress { get; set; }
        public string JsonData { get; set; }
    }
}