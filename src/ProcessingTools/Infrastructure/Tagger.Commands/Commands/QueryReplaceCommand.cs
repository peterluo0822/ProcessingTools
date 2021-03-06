﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Processors.Contracts.Processors;
    using ProcessingTools.Tagger.Commands.Contracts;
    using ProcessingTools.Tagger.Commands.Contracts.Commands;

    [Description("Query replace.")]
    public class QueryReplaceCommand : IQueryReplaceCommand
    {
        private readonly IQueryReplacer queryReplacer;

        public QueryReplaceCommand(IQueryReplacer queryReplacer)
        {
            this.queryReplacer = queryReplacer ?? throw new ArgumentNullException(nameof(queryReplacer));
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
            if (numberOfFileNames < 3)
            {
                throw new ApplicationException("The query file name should be set.");
            }

            string queryFileName = settings.FileNames[2];

            var processedContent = await this.queryReplacer.Replace(document.Xml, queryFileName);

            document.Xml = processedContent;

            return true;
        }
    }
}
