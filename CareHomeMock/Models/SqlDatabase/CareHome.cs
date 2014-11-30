using System;
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
    /// CareHome hasMany CareManagers.
    /// CareHome hasMany MediaFiles.
    /// </summary>
    public class CareHome
    {
        /// <summary>
        /// Professional parameters to display.
        /// </summary>
        [ComplexType]
        public class CareHomeProfessionalParameters
        {
            public int 介護支援専門員在席人数 { get; set; }
            public int 介護支援専門員常勤換算 { get; set; }
            public int 事務員在席人数 { get; set; }
            public int 事務員常勤換算 { get; set; }
            public int その他在席人数 { get; set; }
            public int その他常勤換算 { get; set; }
            public int 全職員在席人数 { get; set; }
            public int 全職員常勤換算 { get; set; }
            public double 経験5年以上割合 { get; set; }

            public double 要介護5 { get; set; }
            public double 要介護4 { get; set; }
            public double 要介護3 { get; set; }
            public double 要介護2 { get; set; }
            public double 要介護1 { get; set; }
            public double 要支援2 { get; set; }
            public double 要支援1 { get; set; }
            public double 自立 { get; set; }

            public double 利用者の権利擁護 { get; set; }
            public double サービスの質の確保 { get; set; }
            public double 相談苦情等への対応 { get; set; }
            public double 外部機関等との連携 { get; set; }
            public double 事業運営管理 { get; set; }
            public double 安全衛生管理等 { get; set; }
            public double 従業者の研修等 { get; set; }
        }

        /// <summary>
        /// Business Key.
        /// </summary>
        [Key]
        public int CareHomeId { get; set; }

        public int CityCode { get; set; }

        /// <summary>
        /// Area which this CareHome belongs to.
        /// </summary>
        [ForeignKey("CityCode")]
        public virtual Area Area { get; set; }

        /// <summary>
        /// Japanese ZIP address.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Japanese address. City, street and more.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Telephone number like 090-1111-1111.
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// Fax number like 090-1111-1112.
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Website URL of this CareHome like "http://example.com/".
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// When this CareHome established.
        /// </summary>
        public DateTime Established { get; set; }

        /// <summary>
        /// Type of company like "営利法人(Corporation)", "非営利法人(NPO)", "自営業(Individual Company)" etc.
        /// </summary>
        public CompanyType CompanyType { get; set; }

        /// <summary>
        /// Name of company like "Foo Corporation".
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Name of chief like "田中太郎".
        /// </summary>
        public string ChiefName { get; set; }

        /// <summary>
        /// Title of chief like "施設長", "最高経営責任者" etc.
        /// </summary>
        public string ChiefJobTitle { get; set; }

        // For CityCode, see above.

        /// <summary>
        /// Longitude of this CareHome. "fX" in CSV.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude of this CareHome. "fY" in CSV.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// When this information is updated. (NOT record.)
        /// </summary>
        public DateTime DataUpdated { get; set; }

        /// <summary>
        /// Professional parameters to display.
        /// </summary>
        public CareHomeProfessionalParameters Parameters { get; set; }

        /// <summary>
        /// RowKey of Image.
        /// </summary>
        public string MediaFileDataId { get; set; }

        /// <summary>
        /// Region of service.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Traits and points of this CareHome.
        /// </summary>
        public string Traits { get; set; }

        /// <summary>
        /// Messages from this CareHome.
        /// </summary>
        public string Messages { get; set; }

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