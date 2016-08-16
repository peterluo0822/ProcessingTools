﻿namespace ProcessingTools.Documents.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ProcessingTools.Common.Models;
    using ProcessingTools.Data.Common.Entity.Models.Contracts;
    using ProcessingTools.Documents.Data.Common.Constants;
    using ProcessingTools.Documents.Data.Common.Models.Contracts;

    public class Affiliation : ModelWithUserInformation, IEntityWithPreJoinedFields, IAffiliationEntity
    {
        private ICollection<Author> authors;

        public Affiliation()
        {
            this.Id = Guid.NewGuid();
            this.authors = new HashSet<Author>();
        }

        [Key]
        public Guid Id { get; set; }

        [MaxLength(ValidationConstants.MaximalLengthOfAffiliationName)]
        public string Name { get; set; }

        public virtual Guid InstitutionId { get; set; }

        public virtual Institution Institution { get; set; }

        public virtual Guid AddressId { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Author> Authors
        {
            get
            {
                return this.authors;
            }

            set
            {
                this.authors = value;
            }
        }

        [NotMapped]
        public IEnumerable<string> PreJoinFieldNames => null;
    }
}