﻿namespace ProcessingTools.MainProgram.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Factories;
    using ProcessingTools.BaseLibrary.Floats;
    using ProcessingTools.Contracts;
    using ProcessingTools.MediaType.Services.Data.Contracts;

    public class ResolveMediaTypesController : TaggerControllerFactory, IResolveMediaTypesController
    {
        private readonly IMediaTypeDataService service;

        public ResolveMediaTypesController(IMediaTypeDataService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            this.service = service;
        }

        protected override async Task Run(XmlDocument document, XmlNamespaceManager namespaceManager, ProgramSettings settings, ILogger logger)
        {
            var parser = new MediaTypesResolver(document.OuterXml, this.service, logger);

            await parser.Parse();

            document.LoadXml(parser.Xml);
        }
    }
}
