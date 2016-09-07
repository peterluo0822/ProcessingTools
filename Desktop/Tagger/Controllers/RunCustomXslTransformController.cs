﻿namespace ProcessingTools.Tagger.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Factories;

    using ProcessingTools.Attributes;
    using ProcessingTools.BaseLibrary;
    using ProcessingTools.Contracts;
    using ProcessingTools.DocumentProvider;
    using ProcessingTools.Processors.Contracts;

    [Description("Custom XSL transform.")]
    public class RunCustomXslTransformController : TaggerControllerFactory, IRunCustomXslTransformController
    {
        private readonly IDocumentXslProcessor processor;

        public RunCustomXslTransformController(IDocumentXslProcessor processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException(nameof(processor));
            }

            this.processor = processor;
        }

        protected override async Task Run(XmlDocument document, XmlNamespaceManager namespaceManager, ProgramSettings settings, ILogger logger)
        {
            int numberOfFileNames = settings.FileNames.Count();

            if (numberOfFileNames < 3)
            {
                throw new ApplicationException("The name of the XSLT file should be set.");
            }

            this.processor.XslFilePath = settings.FileNames.ElementAt(2);

            // TODO: TaxPubDocument should become dummy parameter.
            var context = new TaxPubDocument(document.OuterXml);

            await this.processor.Process(context);

            document.LoadXml(context.Xml);
        }
    }
}