﻿namespace ProcessingTools.Bio.Taxonomy.Processors.Models.Parsers
{
    using System.Collections.Generic;

    internal class MinimalTaxonName : IMinimalTaxonName
    {
        public IEnumerable<IMinimalTaxonNamePart> Parts { get; set; } = new IMinimalTaxonNamePart[] { };

        public override string ToString()
        {
            return string.Join(" | ", this.Parts);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
