﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Commands;
    using ProcessingTools.Attributes;
    using ProcessingTools.Constants;
    using ProcessingTools.Contracts;
    using ProcessingTools.Extensions;
    using ProcessingTools.Processors.Contracts.Processors.References;

    [Description("Tag references.")]
    public class TagReferencesCommand : ITagReferencesCommand
    {
        private readonly IReferencesTagger tagger;

        public TagReferencesCommand(IReferencesTagger tagger)
        {
            if (tagger == null)
            {
                throw new ArgumentNullException(nameof(tagger));
            }

            this.tagger = tagger;
        }

        public Task<object> Run(IDocument document, ICommandSettings settings)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.SetReferencesOutputFileName(document, settings);

            return this.tagger.Tag(document.XmlDocument.DocumentElement);
        }

        private void SetReferencesOutputFileName(IDocument document, ICommandSettings settings)
        {
            string referencesFileName = document.GenerateFileNameFromDocumentId();

            var outputDirectoryName = Path.GetDirectoryName(settings.OutputFileName);
            this.tagger.ReferencesGetReferencesXmlPath = Path.Combine(outputDirectoryName, $"{referencesFileName}-references.{FileConstants.XmlFileExtension}");
        }
    }
}
