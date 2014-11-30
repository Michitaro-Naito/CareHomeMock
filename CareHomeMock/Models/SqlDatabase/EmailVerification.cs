using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Ongoing verification for CareManager.
    /// </summary>
    public class EmailVerification
    {
        /// <summary>
        /// Surrogate ID.
        /// </summary>
        public int EmailVerificationId { get; set; }

        public DateTime Expires { get; set; }

        /// <summary>
        /// GUID, verification code.
        /// </summary>
        public string VerificationCode { get; set; }

        public int CareManagerId { get; set; }

        /// <summary>
        /// CareManager who will be affected by this verification.
        /// </summary>
        [ForeignKey("CareManagerId")]
        public virtual CareManager CareManager { get; set; }
    }
}