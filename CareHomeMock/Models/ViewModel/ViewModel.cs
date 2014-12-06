using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
}