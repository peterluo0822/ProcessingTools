﻿namespace ProcessingTools.Processors.Providers
{
    using System;
    using System.Xml;
    using Contracts.Providers;
    using ProcessingTools.Constants.Content;
    using ProcessingTools.Nlm.Publishing.Constants;

    public class GeographicDeviationTagModelProvider : IGeographicDeviationTagModelProvider
    {
        public Func<XmlDocument, XmlElement> TagModel => document =>
        {
            var tagModel = document.CreateElement(ElementNames.NamedContent);
            tagModel.SetAttribute(AttributeNames.ContentType, ContentTypeConstants.GeographicDeviationContentType);

            return tagModel;
        };
    }
}