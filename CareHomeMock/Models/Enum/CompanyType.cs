using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// 地方自治体／社会福祉法人／医療法人／ＮＰＯ／営利法人／その他法人
    /// </summary>
    public enum CompanyType
    {
        地方自治体,
        社会福祉法人,
        医療法人,
        ＮＰＯ,
        営利法人,
        その他法人,

        生協,
        農協,

        [Display(Name="社団・財団")]
        社団_財団,

        [Display(Name="地方公共団体(都道府県)")]
        地方公共団体_都道府県_,

        [Display(Name="地方公共団体(市区町村)")]
        地方公共団体_市町村_,

        [Display(Name = "地方公共団体(広域連合・一部事務組合等)")]
        地方公共団体_広域連合_一部事務組合等_,

        [Display(Name = "社会福祉法人(社協)")]
        社会福祉法人_社協_,

        [Display(Name = "社会福祉法人(社協以外)")]
        社会福祉法人_社協以外_,

        その他
    }
}