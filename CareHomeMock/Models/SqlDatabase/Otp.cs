﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// One-time-password to post a review.
    /// Generated by CareManager.
    /// Consumed to post by Visitor(Patient or his Family).
    /// </summary>
    public class Otp
    {
        [Key]
        public int OtpId { get; set; }

        /// <summary>
        /// When this OTP will expire and be deleted.
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// 4 digits verification code.
        /// </summary>
        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        [Index(IsUnique=true)]
        public string VerificationCode { get; set; }

        /// <summary>
        /// CareManager generated this OTP for who.
        /// </summary>
        public ReviewerType ReviewerType { get; set; }

        public int CareManagerId { get; set; }

        /// <summary>
        /// CareManager who has generated this OTP.
        /// </summary>
        [ForeignKey("CareManagerId")]
        public virtual CareManager CareManager { get; set; }
    }
}