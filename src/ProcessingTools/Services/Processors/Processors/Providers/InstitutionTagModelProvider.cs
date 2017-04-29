﻿namespace ProcessingTools.Processors.Providers
{
    using System;
    using System.Xml;
    using Contracts.Providers;
    using ProcessingTools.Constants.Content;
    using ProcessingTools.Constants.Schema;

    public class InstitutionTagModelProvider : IInstitutionTagModelProvider
    {
        public Func<XmlDocument, XmlElement> TagModel => document =>
        {
            var tagModel = document.CreateElement(ElementNames.NamedContent);
            tagModel.SetAttribute(AttributeNames.ContentType, ContentTypeConstants.InstitutionContentType);

            return tagModel;
        };
    }
}