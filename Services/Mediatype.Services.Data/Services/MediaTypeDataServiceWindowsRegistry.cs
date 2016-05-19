﻿namespace ProcessingTools.MediaType.Services.Data
{
    using System.Threading.Tasks;

    using Contracts;
    using Factories;

    using Microsoft.Win32;

    public class MediaTypeDataServiceWindowsRegistry : MediaTypeDataServiceFactory, IMediaTypeDataService
    {
        protected override Task<string> ResolveMediaType(string fileExtension)
        {
            return Task.Run(() =>
            {
                string extension = $".{fileExtension.ToLower()}";
                var registryKey = Registry.ClassesRoot.OpenSubKey(extension);
                object value = registryKey?.GetValue("Content Type");

                string mediaType = null;
                if (value != null)
                {
                    mediaType = value.ToString();
                }

                return mediaType;
            });
        }
    }
}