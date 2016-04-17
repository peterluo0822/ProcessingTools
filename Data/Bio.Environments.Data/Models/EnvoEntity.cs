﻿namespace ProcessingTools.Bio.Environments.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ProcessingTools.Bio.Environments.Data.Common.Constants;

    public class EnvoEntity
    {
        private ICollection<EnvoName> envoNames;

        public EnvoEntity()
        {
            this.envoNames = new HashSet<EnvoName>();
        }

        [Key]
        [MinLength(ValidationConstants.MinimalLengthOfEnvoEntityId)]
        [MaxLength(ValidationConstants.MaximalLengthOfEnvoEntityId)]
        public string Id { get; set; }

        public int Index { get; set; }

        [MinLength(ValidationConstants.MinimalLengthOfEnvoId)]
        [MaxLength(ValidationConstants.MaximalLengthOfEnvoId)]
        public string EnvoId { get; set; }

        public virtual ICollection<EnvoName> EnvoNames
        {
            get
            {
                return this.envoNames;
            }

            set
            {
                this.envoNames = value;
            }
        }
    }
}