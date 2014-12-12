using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Japanese area information based on CityCode.
    /// 
    /// Area hasMany CareHomes.
    /// </summary>
    public class Area
    {
        [Key]
        public int AreaId { get; set; }

        /// <summary>
        /// Japanese PrefectureCode like "01".
        /// </summary>
        [Display(Name="都道府県コード")]
        public int PrefectureCode { get; set; }

        /// <summary>
        /// Japanese PrefectureName like "北海道".
        /// </summary>
        [Display(Name="都道府県名")]
        public string PrefectureName { get; set; }

        /// <summary>
        /// Japanese CityCode like "1101".
        /// Business Key.
        /// </summary>
        [Display(Name="市区町村コード")]
        public int CityCode { get; set; }

        /// <summary>
        /// Japanese CityName like "札幌市中央区".
        /// </summary>
        [Display(Name="市区町村名")]
        public string CityName { get; set; }

        /// <summary>
        /// Japanese, medical ZoneCode like "104".
        /// </summary>
        [Display(Name="医療圏コード")]
        public int ZoneCode { get; set; }

        /// <summary>
        /// Japanese, medical ZoneName like "札幌（札幌市など）".
        /// </summary>
        [Display(Name="医療圏名")]
        public string ZoneName { get; set; }

        /// <summary>
        /// Population of medical Zone.
        /// </summary>
        [Display(Name="医療圏人口")]
        public int ZonePopulation { get; set; }

        /// <summary>
        /// CareHomes belongTo this Area.
        /// Lazy loading.
        /// </summary>
        public virtual ICollection<CareHome> CareHomes { get; set; }



        public Area()
        {
            CareHomes = new List<CareHome>();
        }
    }
}