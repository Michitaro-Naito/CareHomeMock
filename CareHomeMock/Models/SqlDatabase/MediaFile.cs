using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Represents a file uploaded by CareHome.
    /// Raw data will go MediaFileData, BlobStorage.
    /// </summary>
    public class MediaFile
    {
        public enum MediaFileType
        {
            [Display(Name="画像")]
            Image = 0,

            [Display(Name="動画")]
            Youtube = 1
        }

        [Key]
        public int MediaFileId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public int CareHomeId { get; set; }
        [ForeignKey("CareHomeId")]
        public virtual CareHome CareHome { get; set; }

        public int Order { get; set; }
        public MediaFileType Type { get; set; }

        public string RowKey { get; set; }

        public string YoutubeUrl { get; set; }

        public string Description { get; set; }



        public MediaFile()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
    }
}