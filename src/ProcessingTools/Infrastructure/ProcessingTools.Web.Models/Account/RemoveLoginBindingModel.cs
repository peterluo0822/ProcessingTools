﻿// <copyright file="RemoveLoginBindingModel.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Binding model for login removal.
    /// </summary>
    public class RemoveLoginBindingModel
    {
        /// <summary>
        /// Gets or sets login provider.
        /// </summary>
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets provider key.
        /// </summary>
        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }
}