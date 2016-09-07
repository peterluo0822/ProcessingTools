﻿namespace ProcessingTools.BaseLibrary.Taxonomy.Models
{
    using System;
    using System.Xml;
    using ProcessingTools.Bio.Taxonomy.Constants;

    public class TaxonNamePart
    {
        private string name;

        public TaxonNamePart()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public TaxonNamePart(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            this.Name = node.InnerText;

            this.Rank = node.Attributes[XmlInternalSchemaConstants.TaxonNamePartRankAttributeName]?.InnerText ?? string.Empty;
            this.Id = node.Attributes[XmlInternalSchemaConstants.IdAttributeName]?.InnerText ?? string.Empty;

            var fullNameAttribute = node.Attributes[XmlInternalSchemaConstants.TaxonNamePartFullNameAttributeName];
            if (fullNameAttribute == null)
            {
                if (this.IsAbbreviated)
                {
                    this.FullName = string.Empty;
                }
                else
                {
                    this.FullName = this.Name;
                }
            }
            else
            {
                this.FullName = fullNameAttribute.InnerText;
            }
        }

        public string Id { get; set; }

        public bool IsAbbreviated { get; private set; }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Contains("."))
                {
                    this.IsAbbreviated = true;
                }
                else
                {
                    this.IsAbbreviated = false;
                }

                this.name = value;
            }
        }

        public string Rank { get; set; }

        public string FullName { get; set; }
    }
}