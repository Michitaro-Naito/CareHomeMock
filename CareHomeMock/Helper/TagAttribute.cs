using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareHomeMock.Helper
{
    public class TagAttribute : Attribute
    {
        /// <summary>
        /// Comma separated tag names like "a" or "a,b,c".
        /// </summary>
        public string Name;

        public string[] Tags
        {
            get
            {
                if(string.IsNullOrEmpty(Name))
                    return new string[0];
                return Name.Split(',');
            }
        }



        public TagAttribute()
        {

        }

        public TagAttribute(string name)
        {
            Name = name;
        }

        public bool Contains(string tagName)
        {
            return Tags.Contains(tagName);
        }
    }
}