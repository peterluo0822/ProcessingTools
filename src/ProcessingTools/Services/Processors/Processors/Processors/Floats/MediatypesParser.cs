﻿namespace ProcessingTools.Processors.Processors.Floats
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using ProcessingTools.Common.Extensions;
    using ProcessingTools.Constants.Schema;
    using ProcessingTools.Processors.Contracts.Models.Floats;
    using ProcessingTools.Processors.Contracts.Processors.Floats;
    using ProcessingTools.Processors.Models.Floats;
    using ProcessingTools.Services.Data.Contracts.Mediatypes;

    public class MediatypesParser : IMediatypesParser
    {
        private readonly IMediatypesResolver mediatypesResolver;

        public MediatypesParser(IMediatypesResolver mediatypesResolver)
        {
            this.mediatypesResolver = mediatypesResolver ?? throw new ArgumentNullException(nameof(mediatypesResolver));
        }

        public async Task<object> Parse(XmlNode context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var mediaElementList = context.SelectNodes(XPathStrings.MediaElement);
            if (mediaElementList.Count < 1)
            {
                return false;
            }

            var extensions = this.GetExtensions(mediaElementList);

            var mediatypes = await this.ResolveMediatypes(extensions);

            foreach (XmlNode mediaNode in mediaElementList)
            {
                var fileName = this.GetFileName(mediaNode);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    continue;
                }

                var mediatype = this.GetResolvedMediatypeForFileName(mediatypes, fileName);

                mediaNode
                    .SetOrUpdateAttribute(AttributeNames.MimeType, mediatype.MimeType)
                    .SetOrUpdateAttribute(AttributeNames.MimeSubtype, mediatype.MimeSubtype);
            }

            return true;
        }

        private IEnumerable<string> GetExtensions(XmlNodeList mediaNodes)
        {
            return mediaNodes.Cast<XmlNode>()
                .Select(this.GetFileName)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Select(this.GetFileExtension)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Distinct()
                .ToArray();
        }

        private string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName).TrimStart('.');
        }

        private string GetFileName(XmlNode mediaNode)
        {
            return mediaNode.Attributes[AttributeNames.XLinkHrefFullAttributeName]?.InnerText;
        }

        private IMediaType GetResolvedMediatypeForFileName(IEnumerable<IMediaType> mediatypes, string fileName)
        {
            IMediaType result = new MediatypeResponseModel
            {
                FileExtension = this.GetFileExtension(fileName)
            };

            if (!string.IsNullOrEmpty(result.FileExtension))
            {
                var mediatype = mediatypes.FirstOrDefault(t => t.FileExtension == result.FileExtension);
                if (mediatype != null)
                {
                    result = mediatype;
                }
            }

            return result;
        }

        private async Task<IMediaType> ResolveFileExtensionToMediatype(string extension)
        {
            var result = new MediatypeResponseModel
            {
                FileExtension = extension
            };

            var response = (await this.mediatypesResolver.ResolveMediatype(extension))
                .FirstOrDefault();

            if (response != null)
            {
                result.MimeType = response.Mimetype;
                result.MimeSubtype = response.Mimesubtype;
            }

            return result;
        }

        private async Task<IEnumerable<IMediaType>> ResolveMediatypes(IEnumerable<string> extensions)
        {
            var result = new HashSet<IMediaType>();
            foreach (var extension in extensions)
            {
                var mediatype = await this.ResolveFileExtensionToMediatype(extension);
                result.Add(mediatype);
            }

            return result;
        }
    }
}
