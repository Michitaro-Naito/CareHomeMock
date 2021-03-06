﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Represents a CareHome in Japan.
    /// 
    /// CareHome belongsTo Area.
    /// CareHome hasMany Applications.
    /// CareHome hasMany CareManagers.
    /// CareHome hasMany MediaFiles.
    /// </summary>
    public class CareHome
    {
        /// <summary>
        /// Surrogate key.
        /// </summary>
        [Key]
        [Display(Name="事業所通し番号")]
        public int CareHomeId { get; set; }

        /// <summary>
        /// Business key.
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Index(IsUnique=true)]
        [Display(Name="事業所ID")]
        public string CareHomeCode { get; set; }

        /// <summary>
        /// Name of CareHome like "Foo Care".
        /// </summary>
        [Index]
        [MaxLength(255)]
        [Display(Name="事業所名")]
        public string Name { get; set; }

        /// <summary>
        /// Is deactivated by Admin?
        /// </summary>
        [Display(Name="無効化")]
        public bool Deactivated { get; set; }

        [Display(Name="市区町村")]
        public int AreaId { get; set; }

        /// <summary>
        /// Area which this CareHome belongs to.
        /// </summary>
        [ForeignKey("AreaId")]
        public virtual Area Area { get; set; }

        /// <summary>
        /// Japanese ZIP address.
        /// </summary>
        [Display(Name="郵便番号")]
        public string Zip { get; set; }

        /// <summary>
        /// Japanese address. City, street and more.
        /// </summary>
        [Display(Name="所在地")]
        public string Address { get; set; }

        /// <summary>
        /// Japanse building name like "○○マンション123号室".
        /// </summary>
        [Display(Name="所在地建物名")]
        public string AddressBuilding { get; set; }

        /// <summary>
        /// Telephone number like 090-1111-1111.
        /// </summary>
        [Display(Name="Tel")]
        public string Tel { get; set; }

        /// <summary>
        /// Fax number like 090-1111-1112.
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Website URL of this CareHome like "http://example.com/".
        /// </summary>
        [Display(Name="公式サイトURL")]
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// When this CareHome established.
        /// </summary>
        [Index]
        [Display(Name="開業日")]
        public DateTime? Established { get; set; }

        [Display(Name="運営年数")]
        public int Years
        {
            get
            {
                var zero = new DateTime(1, 1, 1);
                if (Established == null)
                    return 0;
                try
                {
                    return (zero + (DateTime.UtcNow - Established.Value)).Year - 1;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Type of company like "営利法人(Corporation)", "非営利法人(NPO)", "自営業(Individual Company)" etc.
        /// </summary>
        [Index]
        [Display(Name="法人種別")]
        public CompanyType CompanyType { get; set; }

        /// <summary>
        /// Name of company like "Foo Care, inc.".
        /// </summary>
        [Display(Name="法人名称")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Name of chief like "田中太郎".
        /// </summary>
        [Display(Name="施設管理者氏名")]
        public string ChiefName { get; set; }

        /// <summary>
        /// Title of chief like "施設長", "最高経営責任者" etc.
        /// </summary>
        [Display(Name="施設管理者職名")]
        public string ChiefJobTitle { get; set; }

        // For CityCode, see above.

        /// <summary>
        /// Longitude of this CareHome. "fX" in CSV.
        /// </summary>
        [Index]
        [Display(Name="経度")]
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude of this CareHome. "fY" in CSV.
        /// </summary>
        [Index]
        [Display(Name="緯度")]
        public double Latitude { get; set; }

        /// <summary>
        /// When this information is updated. (NOT record.)
        /// </summary>
        [Display(Name="情報更新日")]
        public DateTime? DataUpdated { get; set; }

        // ----- Professional parameters to display -----
        public double? 介護支援専門員在席人数 { get; set; }
        public double? 介護支援専門員常勤換算 { get; set; }
        public double? 事務員在席人数 { get; set; }
        public double? 事務員常勤換算 { get; set; }
        public double? その他在席人数 { get; set; }
        public double? その他常勤換算 { get; set; }
        public double? 全職員在席人数 { get; set; }
        public double? 全職員常勤換算 { get; set; }
        public double? 経験5年以上割合 { get; set; }

        public double? 利用者数 { get; set; }
        public double? 要介護5 { get; set; }
        public double? 要介護4 { get; set; }
        public double? 要介護3 { get; set; }
        public double? 要介護2 { get; set; }
        public double? 要介護1 { get; set; }
        public double? 要支援2 { get; set; }
        public double? 要支援1 { get; set; }
        public double? 自立 { get; set; }

        public double? 利用者の権利擁護 { get; set; }
        public double? サービスの質の確保 { get; set; }
        public double? 相談苦情等への対応 { get; set; }
        public double? 外部機関等との連携 { get; set; }
        public double? 事業運営管理 { get; set; }
        public double? 安全衛生管理等 { get; set; }
        public double? 従業者の研修等 { get; set; }
        // ----- /Professional parameters to display -----

        [Index]
        public int ReviewCount { get; set; }

        [Index]
        public double Rating { get; set; }

        /// <summary>
        /// Email address like foo@example.com
        /// </summary>
        [Display(Name="メールアドレス")]
        public string Email { get; set; }

        /// <summary>
        /// RowKey of Image.
        /// </summary>
        public string MediaFileDataId { get; set; }

        /// <summary>
        /// Region of service.
        /// </summary>
        [Display(Name="対応可能地域")]
        public string Region { get; set; }

        /// <summary>
        /// Traits and points of this CareHome.
        /// </summary>
        [Index]
        [StringLength(255)]
        [Display(Name="特徴・セールスポイント")]
        public string Traits { get; set; }

        /// <summary>
        /// Messages from this CareHome.
        /// </summary>
        [Display(Name="メッセージ")]
        public string Messages { get; set; }

        /// <summary>
        /// UserId of Owner.
        /// </summary>
        [ForeignKey("User")]
        [MaxLength(128)]
        public string UserId { get; set; }

        /// <summary>
        /// Owner who owns this CareHome. Nullable.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// CareManagers belongs to this CareHome.
        /// </summary>
        public virtual ICollection<CareManager> CareManagers { get; set; }

        /// <summary>
        /// MediaFiles belongs to this CareHome.
        /// </summary>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }



        public CareHome()
        {
            CareManagers = new List<CareManager>();
            MediaFiles = new List<MediaFile>();
        }
    }
}