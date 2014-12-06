using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    public enum ReviewerType
    {
        要介護者本人 = 0,
        要介護者の家族など = 1,
        医師 = 2,
        医療スタッフ = 3,
        介護スタッフ = 4,
        その他の知人 = 5
    }
}