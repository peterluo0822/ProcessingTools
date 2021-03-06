﻿namespace ProcessingTools.DataResources.Data.Entity.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ProcessingTools.Constants.Data.DataResources;
    using ProcessingTools.Contracts.Data.DataResources.Models;
    using ProcessingTools.Contracts.Models;

    public class Abbreviation : EntityWithSources, IAbbreviationEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.AbbreviationNameMaximalLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstants.AbbreviationDefinitionMaximalLength)]
        public string Definition { get; set; }

        public virtual int? ContentTypeId { get; set; }

        public virtual ContentType ContentType { get; set; }

        [NotMapped]
        string IContentTypable.ContentType => this.ContentType.Name;
    }
}
