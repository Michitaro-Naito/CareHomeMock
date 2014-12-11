using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Helper
{
    /// <summary>
    /// Helps to upload / delete files.
    /// </summary>
    public static class BlobHelper
    {
        /// <summary>
        /// Internal object to make this class thread-safe.
        /// </summary>
        static object _lock = new object();

        static CloudStorageAccount _storageAccount = null;
        static CloudBlobClient _blobClient = null;
        static Dictionary<string, CloudBlobContainer> _containers = new Dictionary<string, CloudBlobContainer>();

        /// <summary>
        /// Gets CloudStorageAccount.
        /// Cached static.
        /// </summary>
        public static CloudStorageAccount CloudStorageAccount
        {
            get
            {
                lock (_lock)
                {
                    if (_storageAccount == null)
                        _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                    return _storageAccount;
                }
            }
        }

        /// <summary>
        /// Gets CloudBlobClient.
        /// Cached static.
        /// </summary>
        public static CloudBlobClient CloudBlobClient
        {
            get
            {
                lock (_lock)
                {
                    if (_blobClient == null)
                        _blobClient = CloudStorageAccount.CreateCloudBlobClient();
                    return _blobClient;
                }
            }
        }

        /// <summary>
        /// Gets a CloudBlobContainer for a specific name.
        /// Creates if not exists.
        /// Cached static.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static CloudBlobContainer GetContainer(string name)
        {
            lock (_lock)
            {
                if (_containers.ContainsKey(name))
                    return _containers[name];

                var container = CloudBlobClient.GetContainerReference(name);
                container.CreateIfNotExists();
                _containers.Add(name, container);
                return _containers[name];
            }
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="file">File to upload.</param>
        /// <param name="preferedFilename">Prefered filename.</param>
        /// <returns>Final filename.</returns>
        public static string Upload(string containerName, HttpPostedFileBase file, string preferedFilename)
        {
            var container = GetContainer(containerName);
            var extension = System.IO.Path.GetExtension(preferedFilename).ToLower();

            // Concats GUID to filename to avoid name collision.
            var filename = string.Format("{0}_{1}{2}",
                System.IO.Path.GetFileNameWithoutExtension(preferedFilename),
                Guid.NewGuid(),
                System.IO.Path.GetExtension(preferedFilename));
            var blob = container.GetBlockBlobReference(filename);

            // ContentType from Extension
            switch (extension)
            {
                case ".png":
                    blob.Properties.ContentType = "image/png";
                    break;

                case ".jpg":
                case ".jpeg":
                    blob.Properties.ContentType = "image/jpeg";
                    break;

                case ".gif":
                    blob.Properties.ContentType = "image/gif";
                    break;
            }

            // Uploads (Fails if GUIDs collide. Almost no chance.)
            blob.UploadFromStream(file.InputStream, AccessCondition.GenerateEmptyCondition());

            return filename;
        }

        /// <summary>
        /// Deletes a file if exists.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="filename"></param>
        public static void DeleteIfExists(string containerName, string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            var container = GetContainer(containerName);
            var blobToDelete = container.GetBlockBlobReference(filename);
            blobToDelete.DeleteIfExists();
        }
    }
}