using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Cached values of Review.
    /// Used to calculate CareManager.Rating.
    /// </summary>
    public class ReviewRating
    {
        [Key]
        public int ReviewRatingId { get; set; }

        public DateTime Created { get; set; }

        /// <summary>
        /// ID of reviewed CareManager.
        /// </summary>
        public int CareManagerId { get; set; }

        /// <summary>
        /// Reviewed CareManager.
        /// </summary>
        [ForeignKey("CareManagerId")]
        public virtual CareManager CareManager { get; set; }

        /// <summary>
        /// Cached rating. 1 to 5.
        /// </summary>
        public int Rating { get; set; }



        public ReviewRating()
        {
            Created = DateTime.UtcNow;
        }
    }
}