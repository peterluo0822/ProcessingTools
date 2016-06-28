﻿namespace ProcessingTools.Web.Documents.Areas.Journals.ViewModels.Publishers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ProcessingTools.Documents.Data.Common.Constants;

    public class PublisherViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MaximalLengthOfPublisherName)]
        public string Name { get; set; }

        [MaxLength(ValidationConstants.MaximalLengthOfAbbreviatedPublisherName)]
        public string AbbreviatedName { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
