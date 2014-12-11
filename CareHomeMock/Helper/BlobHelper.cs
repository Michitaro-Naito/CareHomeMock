using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        /// <summary>
        /// Downloads a raw file as a set of byte array and content type.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Tuple<byte[], string> Download(string containerName, string filename)
        {
            var container = GetContainer(containerName);
            var blob = container.GetBlockBlobReference(filename);
            if (!blob.Exists())
                return null;

            using (var mem = new MemoryStream())
            {
                blob.DownloadToStream(mem);
                return Tuple.Create(mem.ToArray(), blob.Properties.ContentType);
            }
        }

        /// <summary>
        /// Downloads a file as an image.
        /// Capable to resize.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="filename"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Bitmap DownloadAsBitmap(string containerName, string filename, int maxWidth = 0, int maxHeight = 0)
        {
            var container = GetContainer(containerName);
            var blob = container.GetBlockBlobReference(filename);
            if (!blob.Exists())
                return null;

            switch (blob.Properties.ContentType)
            {
                case "image/jpeg":
                case "image/png":
                case "image/gif":
                    break;

                default:
                    // This is not an image.
                    return null;
            }

            Bitmap bitmap = null;
            using (var mem = new MemoryStream())
            {
                try
                {
                    blob.DownloadToStream(mem);
                    bitmap = new Bitmap(mem);
                }
                catch
                {
                    return null;
                }
            }

            if (maxWidth != 0 || maxHeight != 0)
            {
                // Resize
                var w = (double)bitmap.Width;
                var h = (double)bitmap.Height;
                var aspect = w / h;

                if (maxWidth != 0 && maxWidth < w)
                {
                    var scale = (maxWidth / w);
                    w *= scale;
                    h *= scale;
                }

                if (maxHeight != 0 && maxHeight < h)
                {
                    var scale = (maxHeight / h);
                    w *= scale;
                    h *= scale;
                }

                if(w != bitmap.Width || h != bitmap.Height)
                    bitmap = new Bitmap(bitmap, new Size((int)w, (int)h));
            }

            return bitmap;
        }
    }
}