using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Helper for Table Storage.
    /// Helps to get a table, insert, query etc.
    /// All static members are thread-safe.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class TableHelper<T>
        where T : TableEntity
    {
        static object _lock = new object();
        static Dictionary<string, CloudTable> _tables = new Dictionary<string, CloudTable>();
        public static CloudTable Table
        {
            get
            {
                lock (_lock)
                {
                    var name = typeof(T).Name;
                    if (_tables.Keys.Contains(name))
                        return _tables[name];

                    Debug.WriteLine("Getting table: " + name);
                    var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                    var tableClient = storageAccount.CreateCloudTableClient();
                    var table = tableClient.GetTableReference(name);
                    table.CreateIfNotExists();
                    _tables[name] = table;
                    //Thread.Sleep(10000);

                    return _tables[name];
                }
            }
        }
    }

    public static class TableExtension
    {
        public static void Insert(this CloudTable table, List<ITableEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return;

            if (entities.Count == 1)
            {
                // Single entity.
                table.Execute(TableOperation.Insert(entities[0]));
                return;
            }

            // Multiple entities.
            var threadAmount = (int)Math.Ceiling(entities.Count / 100.0);
            Parallel.For(0, threadAmount, threadIndex =>
            {
                //Debug.WriteLine("Thread {0}", threadIndex);
                var batch = new TableBatchOperation();
                for (var n = 100 * threadIndex; n < 100 * (threadIndex + 1) && n < entities.Count; n++)
                {
                    batch.Insert(entities[n]);
                }
                table.ExecuteBatch(batch);
            });
        }

        public static void Insert(this CloudTable table, params ITableEntity[] entities)
        {
            if (entities == null || entities.Length == 0)
                return;

            table.Insert(entities.ToList());
        }
    }
}