﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareHomeMock.Models
{
    /// <summary>
    /// Application(texts) sent by a visitor.
    /// 
    /// A CareHome owner sends an application to admins to become a CareHomeUser.
    /// An Admin accepts / denies.
    /// 
    /// Application belongsTo CareHome.
    /// </summary>
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        public int? CareHomeId { get; set; }

        /// <summary>
        /// CareHome which this Application belongs to.
        /// Lazy loading.
        /// </summary>
        [ForeignKey("CareHomeId")]
        public virtual CareHome CareHome { get; set; }

        /// <summary>
        /// Email address of sender.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Fullnames of sender.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Notes from sender to Admin.
        /// </summary>
        public string Note { get; set; }
    }
}