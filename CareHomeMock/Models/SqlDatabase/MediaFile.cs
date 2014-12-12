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

        [Display(Name="作成日時")]
        public DateTime Created { get; set; }

        [Display(Name="更新日時")]
        public DateTime Updated { get; set; }

        public int CareHomeId { get; set; }

        [ForeignKey("CareHomeId")]
        public virtual CareHome CareHome { get; set; }

        [Display(Name="順序")]
        public int Order { get; set; }

        [Display(Name="種別")]
        public MediaFileType Type { get; set; }

        [Display(Name="ファイル名")]
        public string RowKey { get; set; }

        [Display(Name="動画ID")]
        public string YoutubeUrl { get; set; }

        [Display(Name="紹介文")]
        public string Description { get; set; }



        public MediaFile()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
    }
}