﻿namespace ProcessingTools.Tagger.Controllers
{
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Factories;

    using ProcessingTools.Attributes;
    using ProcessingTools.BaseLibrary.Floats;
    using ProcessingTools.Contracts;

    [Description("Tag floats.")]
    public class TagFloatsController : TaggerControllerFactory, ITagFloatsController
    {
        protected override async Task Run(IDocument document, XmlNamespaceManager namespaceManager, ProgramSettings settings, ILogger logger)
        {
            var tagger = new FloatsTagger(document.Xml, logger);

            await tagger.Tag();

            document.Xml = tagger.Xml;
        }
    }
}