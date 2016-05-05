﻿namespace ProcessingTools.Tagger.Models
{
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using ProcessingTools.Nlm.Publishing.Constants;

    [XmlType(AnonymousType = true)]
    [XmlRoot(NodeNames.NamedContent, Namespace = "", IsNullable = false)]
    public class BiorepositoryInstitutionSerializableModel
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(AttributeNames.ContentType)]
        public string ContentType
        {
            get
            {
                return "institution";
            }

            set
            {
            }
        }

        [XmlAttribute(AttributeNames.XLinkType, Form = XmlSchemaForm.Qualified, Namespace = Namespaces.XlinkNamespace)]
        public string XLinkType
        {
            get
            {
                return ProcessingTools.Nlm.Publishing.Types.XLinkType.Simple.ToString().ToLower();
            }

            set
            {
            }
        }

        [XmlAttribute(AttributeNames.XLinkHref, Form = XmlSchemaForm.Qualified, Namespace = Namespaces.XlinkNamespace)]
        public string Url { get; set; }
    }
}
