﻿namespace ProcessingTools.Tagger.Commands.Generics
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Commands;
    using ProcessingTools.Contracts;
    using ProcessingTools.Layout.Processors.Contracts.Normalizers;

    public class GenericDocumentTaggerWithNormalizationCommand<TTagger> : ITaggerCommand
        where TTagger : IDocumentTagger
    {
        private readonly TTagger tagger;
        private readonly IDocumentSchemaNormalizer documentNormalizer;

        public GenericDocumentTaggerWithNormalizationCommand(TTagger tagger, IDocumentSchemaNormalizer documentNormalizer)
        {
            if (tagger == null)
            {
                throw new ArgumentNullException(nameof(tagger));
            }

            if (documentNormalizer == null)
            {
                throw new ArgumentNullException(nameof(documentNormalizer));
            }

            this.tagger = tagger;
            this.documentNormalizer = documentNormalizer;
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

            var result = await this.tagger.Tag(document);
            await this.documentNormalizer.NormalizeToSystem(document);

            return result;
        }
    }
}
