﻿namespace ProcessingTools.Bio.Taxonomy.ServiceClient.GlobalNamesResolver.Models.Xml
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "canonical-form", Namespace = "", IsNullable = false)]
    public class HashDataDatumResultsResultCanonicalForm
    {
        [XmlText]
        public string Value { get; set; }
    }
}
