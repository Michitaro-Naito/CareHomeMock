using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace CareHomeMock.Models
{
    public enum LogType
    {
        Fatal = 0,
        Admin = 1,
        CareHome = 2,
        CareManager = 3,
        Others = 4
    }

    /// <summary>
    /// Represents a Log.
    /// Describes what happened on this website.
    /// </summary>
    public class Log : TableEntity
    {
        // PartitionKey = Kind(Fatal,Admin,CareHome,CareManager,Others)
        public LogType LogType
        {
            get
            {
                var type = LogType.Fatal;
                Enum.TryParse<LogType>(PartitionKey, out type);
                return type;
            }
            set
            {
                PartitionKey = value.ToString();
            }
        }
        // RowKey = Timestamp + GUID, Descending

        public DateTime Created { get; set; }

        // Layer 4
        public string IpAddress { get; set; }

        // Layer 7
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public int ContentLength { get; set; }
        public string Cookies { get; set; }
        public string QueryString { get; set; }
        public string Form { get; set; }

        // Application Layer
        public string UserId { get; set; }
        public string Action { get; set; }
        public string JsonData { get; set; }

        public Log()
        {
            Created = DateTime.UtcNow;
            RowKey = string.Format("{0:D19}_{1}", (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks), Guid.NewGuid());
        }
    }
}