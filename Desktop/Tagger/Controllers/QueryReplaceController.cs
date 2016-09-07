﻿namespace ProcessingTools.Tagger.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Factories;

    using ProcessingTools.Attributes;
    using ProcessingTools.Contracts;
    using ProcessingTools.Processors.Contracts;

    [Description("Query replace.")]
    public class QueryReplaceController : TaggerControllerFactory, IQueryReplaceController
    {
        private readonly IQueryReplacer queryReplacer;

        public QueryReplaceController(IQueryReplacer queryReplacer)
        {
            if (queryReplacer == null)
            {
                throw new ArgumentNullException(nameof(queryReplacer));
            }

            this.queryReplacer = queryReplacer;
        }

        protected override async Task Run(XmlDocument document, XmlNamespaceManager namespaceManager, ProgramSettings settings, ILogger logger)
        {
            int numberOfFileNames = settings.FileNames.Count();

            if (numberOfFileNames < 3)
            {
                throw new ApplicationException("The query file name should be set.");
            }

            string queryFileName = settings.FileNames.ElementAt(2);

            var processedContent = await this.queryReplacer.Replace(document.OuterXml, queryFileName);

            document.LoadXml(processedContent);
        }
    }
}