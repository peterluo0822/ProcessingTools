﻿namespace ProcessingTools.BaseLibrary.Floats
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using Configurator;
    using Contracts.Log;
    using MediaType.Services.Data.Contracts;
    using Models;

    public class MediaTypesResolver : Base, IBaseParser
    {
        private const string DefaultMimeType = "unknown";
        private const string DefaultMimeSubtype = "unknown";

        private const string MediaElementXPath = "//media";

        private const string FileNameAttributeName = "xlink:href";
        private const string MimeTypeAttributeName = "mimetype";
        private const string MimeSubtypeAttributeName = "mime-subtype";

        private ILogger logger;

        private IMediaTypeDataService mediatypeDataService;

        public MediaTypesResolver(Config config, string xml, IMediaTypeDataService mediatypeDataService, ILogger logger)
            : base(config, xml)
        {
            this.mediatypeDataService = mediatypeDataService;
            this.logger = logger;
        }

        public MediaTypesResolver(IBase baseObject, IMediaTypeDataService mediatypeDataService, ILogger logger)
            : base(baseObject)
        {
            this.mediatypeDataService = mediatypeDataService;
            this.logger = logger;
        }

        public void Parse()
        {
            XmlNodeList mediaElements = this.XmlDocument.SelectNodes(MediaElementXPath, this.NamespaceManager);

            var extensions = new HashSet<string>(mediaElements.Cast<XmlNode>()
                .Select(m => m.Attributes[FileNameAttributeName]?.InnerText)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Select(f => Path.GetExtension(f).TrimStart('.'))
                .Where(e => !string.IsNullOrWhiteSpace(e)));

            var types = new HashSet<MediaTypeResponseModel>(extensions.Select(e =>
            {
                var result = new MediaTypeResponseModel
                {
                    FileExtension = e,
                    MimeType = DefaultMimeType,
                    MimeSubtype = DefaultMimeSubtype
                };

                var response = this.mediatypeDataService.GetMediaType(e)?.FirstOrDefault();
                if (response != null)
                {
                    result.FileExtension = response.FileExtension;
                    result.MimeType = response.MimeType;
                    result.MimeSubtype = response.MimeSubtype;
                }

                return result;
            }));

            foreach (XmlNode mediaElement in mediaElements)
            {
                string mimeType = DefaultMimeType;
                string mimeSubtype = DefaultMimeSubtype;

                var fileName = mediaElement.Attributes[FileNameAttributeName]?.InnerText;
                if (fileName != null && !string.IsNullOrWhiteSpace(fileName))
                {
                    string fileExtension = Path.GetExtension(fileName).TrimStart('.');
                    if (fileExtension.Length > 0)
                    {
                        var result = types?.FirstOrDefault(t => t.FileExtension == fileExtension);
                        if (result != null)
                        {
                            mimeType = result.MimeType;
                            mimeSubtype = result.MimeSubtype;
                        }
                    }

                    this.SetMimeAttribute(mediaElement, MimeTypeAttributeName, mimeType);
                    this.SetMimeAttribute(mediaElement, MimeSubtypeAttributeName, mimeSubtype);
                }
                else
                {
                    this.logger?.Log(LogType.Warning, "File name not provided.");
                }
            }
        }

        private void SetMimeAttribute(XmlNode mediaElement, string attributeName, string type)
        {
            var mimeAttribute = mediaElement.Attributes[attributeName];
            if (mimeAttribute == null)
            {
                var attribute = mediaElement.OwnerDocument.CreateAttribute(attributeName);
                mediaElement.Attributes.Append(attribute);
                mimeAttribute = attribute;
            }

            mimeAttribute.InnerText = type;
        }
    }
}