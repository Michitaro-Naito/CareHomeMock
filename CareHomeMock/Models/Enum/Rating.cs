using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public string Label { get; set; }
    }
}