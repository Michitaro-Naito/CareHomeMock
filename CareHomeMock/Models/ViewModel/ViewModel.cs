﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CareHomeMock.Models
{
    public class CareManagerIndexVM
    {
        public int CareHomeId { get; set; }
        public List<CareHomeMock.Models.CareManager> CareManagers { get; set; }
    }

    public class HomePostReviewVM
    {
        [Required]
        public int CareManagerId { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        [Display(Name="パスワード")]
        public string Otp { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name="評価点数")]
        public int Rating { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(1024)]
        [Display(Name="コメント")]
        public string Message { get; set; }

        public HomePostReviewVM()
        {
            Rating = 5;
        }
    }

    public class MediaFileUploadVM
    {
        /// <summary>
        /// MediaFileId to overwrite. Nullable.
        /// </summary>
        public int MediaFileId { get; set; }

        /// <summary>
        /// CareHomeId which this file belongs to.
        /// </summary>
        public int CareHomeId { get; set; }

        /// <summary>
        /// Type of media. Image or Youtube.
        /// </summary>
        public MediaFile.MediaFileType MediaFileType { get; set; }

        /// <summary>
        /// Image file to upload.
        /// </summary>
        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// URL of the Youtube movie like "http://youtu.be/gYjkPo7DcWI?list=UUOXg188gzqJSdVv0MiVA7SA" or "https://www.youtube.com/watch?v=gYjkPo7DcWI".
        /// </summary>
        public string YoutubeUrl { get; set; }

        /// <summary>
        /// Returns the "v" value of YoutubeUrl.
        /// Returns null if invalid.
        /// </summary>
        public string YoutubeVValue
        {
            get
            {
                if (YoutubeUrl == null)
                    return null;
                var regex = new Regex(@"^https?:\/\/(www\.youtube\.com\/watch\?v=(?<v>[a-zA-Z0-9-_]+))|(youtu.be/(?<v>[a-zA-Z0-9-_]+))");
                var match = regex.Match(YoutubeUrl);
                if (!match.Success || match.Groups["v"] == null)
                    return null;
                return match.Groups["v"].ToString();
            }
        }

        /// <summary>
        /// Description of this file like "We are doing like this!".
        /// </summary>
        public string Description { get; set; }
    }
}