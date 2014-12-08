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
using System.Diagnostics;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text.RegularExpressions;

namespace CareHomeMock.Controllers
{
    public class MediaFileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// CareHome views and arranges it's files.
        /// </summary>
        /// <param name="careHomeId"></param>
        /// <returns></returns>
        public ActionResult Index(int? careHomeId)
        {
            if (careHomeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var home = db.CareHomes.Find(careHomeId);
            if (home == null)
                return HttpNotFound();

            var mediafiles = home.MediaFiles.OrderBy(f=>f.Order).ThenByDescending(f=>f.Updated).ToList();
            return View(mediafiles);
        }

        /// <summary>
        /// CareHome arranges it's files. AJAX.
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveOrder(int[] orderedIds)
        {
            // TODO: check UserRole.
            var rows = db.MediaFiles.Where(f => orderedIds.Contains(f.MediaFileId)).ToList();
            var nextOrder = 0;
            foreach (var id in orderedIds)
            {
                var row = rows.FirstOrDefault(r => r.MediaFileId == id);
                if (row != null)
                {
                    row.Order = nextOrder;
                    nextOrder++;
                }
            }
            db.SaveChanges();

            return Json(new { result = "success" });
        }

        /// <summary>
        /// CareHome uploads a file.
        /// If id is specified, overwrites an existing file.
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(int careHomeId, int mediaFileId)
        {
            if (careHomeId == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var careHome = db.CareHomes.Find(careHomeId);
            if (careHome == null)
                return HttpNotFound();

            MediaFile existing = null;
            if (mediaFileId != 0)
            {
                existing = careHome.MediaFiles.FirstOrDefault(f => f.MediaFileId == mediaFileId);
                if (existing == null)
                    return HttpNotFound();
            }

            var model = new MediaFileUploadVM() { MediaFileId = mediaFileId, CareHomeId = careHomeId };
            if (existing != null)
            {
                model.MediaFileType = existing.Type;
                model.Description = existing.Description;
            }

            ViewBag.MediaFileType = Helper.Helper.MediaFileTypes;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(MediaFileUploadVM model)
        {
            MediaFile existing = null;
            if (model.MediaFileId != 0)
            {
                existing = db.MediaFiles.Find(model.MediaFileId);
                if(existing == null)
                    throw new Exception(string.Format("上書きしようとしたファイルが見つかりませんでした。ID:{0}", model.MediaFileId));
                if (existing.CareHomeId != model.CareHomeId)
                    throw new Exception("上書きしようとしたファイルは指定された事業所のものではありません。");
            }

            // Validates
            if (existing == null)
            {
                // Add
                if (model.MediaFileType == MediaFile.MediaFileType.Image && model.File == null)
                    ModelState.AddModelError("File", "選択してください。");
                if (model.MediaFileType == MediaFile.MediaFileType.Youtube && model.YoutubeVValue == null)
                    ModelState.AddModelError("YoutubeUrl", "正しいYoutube動画のURLを入力してください。");
                var home = db.CareHomes.Find(model.CareHomeId);
                if (home == null)
                    throw new Exception("該当する事業所が見つかりません。");
                if (home.MediaFiles.Count >= 100)
                    ModelState.AddModelError("", "追加できるファイルの最大数を超えています。削除してからお試しください。");
            }
            else
            {
                // Edit
                if (model.MediaFileType == MediaFile.MediaFileType.Image && model.File == null && string.IsNullOrEmpty(existing.RowKey))
                    ModelState.AddModelError("File", "選択してください。");
                if (model.MediaFileType == MediaFile.MediaFileType.Youtube && model.YoutubeVValue == null && !string.IsNullOrEmpty(model.YoutubeUrl))
                    ModelState.AddModelError("YoutubeUrl", "正しいYoutube動画のURLを入力するか空欄にしてください。");
            }

            if (ModelState.IsValid)
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("mediafile");
                container.CreateIfNotExists();

                var file = model.File;

                // Deletes an existing file first.
                if (existing != null
                    && ((model.MediaFileType == MediaFile.MediaFileType.Image && file != null)
                    || (model.MediaFileType == MediaFile.MediaFileType.Youtube)))
                {
                    try
                    {
                        var blobToDelete = container.GetBlockBlobReference(existing.RowKey);
                        blobToDelete.Delete();
                    }
                    catch
                    {
                        // Thrown when Blob is not there. Does nothing.
                    }
                }

                // FileName
                string fileName;
                if (model.MediaFileType == MediaFile.MediaFileType.Youtube)
                {
                    fileName = null;
                }
                else
                {
                    if (file != null)
                    {
                        // Picks a new filename for the new file.
                        fileName = file.FileName;
                        var sameName = db.MediaFiles.FirstOrDefault(f => f.RowKey == fileName);
                        if (sameName != null)
                        {
                            fileName = string.Format("{0}_{1}{2}",
                                System.IO.Path.GetFileNameWithoutExtension(fileName),
                                Guid.NewGuid(),
                                System.IO.Path.GetExtension(fileName));
                        }
                    }
                    else
                    {
                        // Uses an existing filename.
                        fileName = existing.RowKey;
                    }
                }

                // Updates SQL
                MediaFile row = existing;
                if (row == null)
                {
                    row = new MediaFile();
                    db.MediaFiles.Add(row);
                }
                row.Updated = DateTime.UtcNow;
                row.CareHomeId = model.CareHomeId;
                row.Type = model.MediaFileType;
                row.RowKey = fileName;
                if(model.YoutubeVValue != null)
                    row.YoutubeUrl = model.YoutubeVValue;
                row.Description = model.Description;
                db.SaveChanges();

                if (model.MediaFileType == MediaFile.MediaFileType.Image && file != null)
                {
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
                }

                // Done. Redirects User to Index.
                return RedirectToAction("Index", new { careHomeId = model.CareHomeId });
            }

            ViewBag.MediaFileType = Helper.Helper.MediaFileTypes;
            return View(model);
        }

        /// <summary>
        /// Anybody downloads a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult Download(string fileName)
        {
            if (fileName == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Debug.WriteLine("Downloading: " + fileName);
            //var file = db.Files.Find(fileName);
            //if (file == null)
            //    return HttpNotFound();

            // TODO: cache this
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("mediafile");
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

        /// <summary>
        /// CareHome deletes it's file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaFile mediafile = db.MediaFiles.Find(id);
            if (mediafile == null)
            {
                return HttpNotFound();
            }
            return View(mediafile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MediaFile mediafile = db.MediaFiles.Find(id);

            // Removes from Blob
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("mediafile");
            container.CreateIfNotExists();

            try
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(mediafile.RowKey);
                blockBlob.Delete();
            }
            catch
            {
                // Does nothing.
            }

            // Removes from SQL
            db.MediaFiles.Remove(mediafile);
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
