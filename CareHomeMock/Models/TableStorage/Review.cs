﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Represents a Review which has been posted by Visitor.
    /// </summary>
    public class Review : TableEntity
    {
        // PartitionKey = CareManagerId
        // RowKey = TimeStamp + GUID

        public DateTime Created { get; set; }
        public string IpAddress { get; set; }
        public string Host { get; set; }

        public ReviewerType PatientType { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string Reply { get; set; }
    }
}