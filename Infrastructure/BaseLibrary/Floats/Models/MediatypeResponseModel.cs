﻿namespace ProcessingTools.BaseLibrary.Floats.Models
{
    using ProcessingTools.Constants.Media;

    internal class MediaTypeResponseModel : IMediaType
    {
        public string FileExtension { get; set; }

        public string MimeType { get; set; } = MediaTypes.DefaultMimeType;

        public string MimeSubtype { get; set; } = MediaTypes.DefaultMimeSubtype;
    }
}
