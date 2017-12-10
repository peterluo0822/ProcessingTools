﻿namespace ProcessingTools.Tagger.Commands
{
    using System;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Commands;
    using ProcessingTools.Contracts.Commands.Tagger;
    using ProcessingTools.Contracts.Processors;

    public class XmlContextTaggerCommand<TTagger> : ITaggerCommand
        where TTagger : class, IXmlContextTagger
    {
        private readonly TTagger tagger;

        public XmlContextTaggerCommand(TTagger tagger)
        {
            this.tagger = tagger ?? throw new ArgumentNullException(nameof(tagger));
        }

        public Task<object> RunAsync(IDocument document, ICommandSettings settings)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return this.tagger.TagAsync(document.XmlDocument.DocumentElement);
        }
    }
}