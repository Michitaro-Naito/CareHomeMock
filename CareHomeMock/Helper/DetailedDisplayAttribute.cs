using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareHomeMock.Helper
{
    public class DetailedDisplayAttribute : Attribute
    {
        public bool Readonly;

        public string Placeholder;
    }
}