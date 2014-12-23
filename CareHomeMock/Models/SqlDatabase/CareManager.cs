using CareHomeMock.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Represents a CareManager in Japan.
    /// 
    /// CareManager belongsTo CareHome.
    /// CareManager hasMany Otps.
    /// CareManager hasMany EmailVerifications.
    /// CareManager hasMany Reviews. (NoSQL)
    /// </summary>
    public class CareManager
    {
        /// <summary>
        /// Incremental, surrogate identifier.
        /// </summary>
        public int CareManagerId { get; set; }

        public int CareHomeId { get; set; }

        /// <summary>
        /// CareHome which this CareManager belongs to.
        /// </summary>
        [ForeignKey("CareHomeId")]
        public virtual CareHome CareHome { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// RowKey of Image.
        /// </summary>
        public string MediaFileDataId { get; set; }

        /// <summary>
        /// Email address like foo@example.com
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name="メールアドレス", Description="非公表")]
        [DetailedDisplay(Placeholder="例: foo@example.com")]
        public string Email { get; set; }

        /// <summary>
        /// Birthday of this CareManager. Date only like 2014/1/1 0:00:00.
        /// Always Month = 12, Day = 31
        /// </summary>
        [Required]
        [Display(Name = "生年月日", Description="年齢に変換して公表")]
        [DetailedDisplay(Placeholder="例: 2014/1/1")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Name of this CareManager like "田中太郎".
        /// </summary>
        [Display(Name = "氏名", Description = "公表 ※個人情報を気になさる方は本名でなくてもかまいません（姓のみ、ニックネームなど）")]
        [DetailedDisplay(Placeholder="例: 田中一郎")]
        public string Name { get; set; }

        /// <summary>
        /// Gender of this CareManager.
        /// </summary>
        [Display(Name="性別",Description="公表")]
        [DetailedDisplay(Placeholder="例: 男性")]
        public Gender Gender { get; set; }

        /// <summary>
        /// Age of this CareManager like 24.
        /// </summary>
        [Display(Name="年齢")]
        public int Age
        {
            get
            {
                try
                {
                    var zero = new DateTime(1, 1, 1);
                    return (zero + (DateTime.UtcNow - Birthday)).Year - 1;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// When this CareManager has been licensed.
        /// Always Month = 12, Day = 31
        /// </summary>
        [Required]
        [Display(Name = "ケアマネ資格取得年",Description="公表")]
        [DetailedDisplay(Placeholder="例: 2014/1/1")]
        public DateTime Licensed { get; set; }

        /// <summary>
        /// How many full years passed sinse licensed.
        /// </summary>
        [Display(Name="経験年数")]
        public int Years
        {
            get
            {
                try
                {
                    var zero = new DateTime(1, 1, 1);
                    return (zero + (DateTime.UtcNow - Licensed)).Year - 1;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Amount of current patients.
        /// </summary>
        [Display(Name="担当人数")]
        [DetailedDisplay(Placeholder="例: 12")]
        public int CurrentPatients { get; set; }

        /// <summary>
        /// Does this CareManager allows new patient?
        /// </summary>
        [Display(Name="新規対応可")]
        public bool AllowNewPatient { get; set; }

        /// <summary>
        /// Career of this CareManager.
        /// </summary>
        [Display(Name="経歴")]
        public string Career { get; set; }

        /// <summary>
        /// Messages from this CareManager.
        /// </summary>
        [Display(Name="メッセージ")]
        public string Messages { get; set; }

        /// <summary>
        /// Blog URLs of this CareManager.
        /// </summary>
        [Display(Name="ブログのURLなど")]
        public string BlogUrls { get; set; }

        [Display(Name="評価を公開する")]
        public bool ShowReviews { get; set; }

        // ----- Professional Parameters to Display -----
        [Range(1, 5)]
        public double 企画立案力 { get; set; }
        public double 行動実践力 { get; set; }
        public double 関係構築力 { get; set; }
        public double マネジメント力 { get; set; }
        public double 指導管理力 { get; set; }
        [Display(Name="公平中立性")]
        public double 公平中立力 { get; set; }
        [Display(Name="医療の知識")]
        public double 医療知識 { get; set; }
        [Display(Name="介護の知識")]
        public double 介護知識 { get; set; }
        // ----- /Professional Parameters to Display -----

        /// <summary>
        /// Cached total rating.
        /// </summary>
        public double TotalRating { get; set; }

        /// <summary>
        /// Cached amount of ratings.
        /// </summary>
        public int ReviewsCount { get; set; }

        /// <summary>
        /// Cached average rating.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// UserId of Owner.
        /// </summary>
        [ForeignKey("User")]
        [MaxLength(128)]
        public string UserId { get; set; }

        /// <summary>
        /// Owner who owns this CareManager. Nullable.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// OTPs generated by this CareManager.
        /// </summary>
        public virtual ICollection<Otp> Otps { get; set; }

        /// <summary>
        /// Ongoing EmailVerifications for this CareManager.
        /// </summary>
        public virtual ICollection<EmailVerification> EmailVerifications { get; set; }

        /// <summary>
        /// CareManager HABTM Licenses.
        /// </summary>
        public virtual ICollection<CareManagerLicenses> CareManagerLicenses { get; set; }

        /// <summary>
        /// Cached ratings to calculate this.Rating.
        /// </summary>
        public virtual ICollection<ReviewRating> ReviewRatings { get; set; }



        public CareManager()
        {
            Licensed = DateTime.UtcNow;
            Birthday = DateTime.UtcNow;
            Otps = new List<Otp>();
            EmailVerifications = new List<EmailVerification>();
            CareManagerLicenses = new List<CareManagerLicenses>();

            企画立案力 = 1;
            行動実践力 = 1;
            関係構築力 = 1;
            指導管理力 = 1;
            公平中立力 = 1;
            医療知識 = 1;
            介護知識 = 1;
        }
    }
}