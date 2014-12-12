using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CareHomeMock.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure;
using System.Diagnostics;

namespace CareHomeMock.Controllers
{
    /// <summary>
    /// Admin manages uploaded files here.
    /// </summary>
    public class FileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /File/
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            return View(db.Files.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Upload()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            Debug.WriteLine("{0} bytes uploaded.", file.ContentLength);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("file");
            container.CreateIfNotExists();

            // Checks for name collisions.
            var fileName = file.FileName;
            var existing = db.Files.FirstOrDefault(f=>f.RowKey==fileName);
            if (existing != null)
            {
                fileName = string.Format("{0}_{1}{2}",
                    System.IO.Path.GetFileNameWithoutExtension(fileName),
                    Guid.NewGuid(),
                    System.IO.Path.GetExtension(fileName));
            }

            // Inserts to SQL.
            var row = new File()
            {
                RowKey = fileName
            };
            db.Files.Add(row);
            db.SaveChanges();

            // Uploads to Blob.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            switch (System.IO.Path.GetExtension(fileName).ToLower())
            {
                case ".png":
                    blockBlob.Properties.ContentType = "image/png";
                    break;

                case ".jpg":
                case ".jpeg":
                    blockBlob.Properties.ContentType = "image/jpeg";
                    break;

                case ".gif":
                    blockBlob.Properties.ContentType = "image/gif";
                    break;
            }
            blockBlob.UploadFromStream(file.InputStream);

            Debug.WriteLine("Maybe uploaded to blob." + fileName);

            return null;
        }

        public ActionResult Download(string fileName)
        {
            Debug.WriteLine("Downloading: " + fileName);
            //var file = db.Files.Find(fileName);
            //if (file == null)
            //    return HttpNotFound();

            // TODO: cache this
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("file");
            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            if (!blockBlob.Exists())
                return HttpNotFound();

            using (var mem = new System.IO.MemoryStream())
            {
                blockBlob.DownloadToStream(mem);
                return File(mem.ToArray(), blockBlob.Properties.ContentType);
            }
        }

        // GET: /File/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: /File/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            File file = db.Files.Find(id);

            // Deletes from Blob
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("file");
            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.RowKey);
            blockBlob.Delete();

            // Deletes from SQL
            db.Files.Remove(file);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
