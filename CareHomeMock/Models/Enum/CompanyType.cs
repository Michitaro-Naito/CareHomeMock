using System;
using System.Collections.Generic;
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
        社団_財団,
        地方公共団体_都道府県_,
        地方公共団体_市町村_,
        地方公共団体_広域連合_一部事務組合等_,
        社会福祉法人_社協_,
        社会福祉法人_社協以外_,
        その他
    }
}