﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Files.IO;
    using ProcessingTools.Processors.Contracts.Processors.Bio.ZooBank;
    using ProcessingTools.Tagger.Commands.Contracts;
    using ProcessingTools.Tagger.Commands.Contracts.Commands;

    [Description("Clone ZooBank XML.")]
    public class ZooBankCloneXmlCommand : IZooBankCloneXmlCommand
    {
        private readonly IZoobankXmlCloner cloner;
        private readonly IDocumentFactory documentFactory;
        private readonly IXmlFileReader fileReader;

        public ZooBankCloneXmlCommand(IDocumentFactory documentFactory, IZoobankXmlCloner cloner, IXmlFileReader fileReader)
        {
            this.documentFactory = documentFactory ?? throw new ArgumentNullException(nameof(documentFactory));
            this.cloner = cloner ?? throw new ArgumentNullException(nameof(cloner));
            this.fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
        }

        public async Task<object> Run(IDocument document, ICommandSettings settings)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            int numberOfFileNames = settings.FileNames.Count;
            if (numberOfFileNames < 2)
            {
                throw new ApplicationException("Output file name should be set.");
            }

            if (numberOfFileNames < 3)
            {
                throw new ApplicationException("The file path to xml-file-to-clone should be set.");
            }

            string sourceFileName = settings.FileNames[2];
            var sourceDocument = await this.ReadSourceDocument(sourceFileName);

            return await this.cloner.Clone(document, sourceDocument);
        }

        private async Task<IDocument> ReadSourceDocument(string sourceFileName)
        {
            var xml = await this.fileReader.ReadXml(sourceFileName);
            var document = this.documentFactory.Create(xml.OuterXml);
            return document;
        }
    }
}
