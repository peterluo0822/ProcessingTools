﻿namespace ProcessingTools.Tagger.Controllers.Generics
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Controllers;
    using ProcessingTools.Contracts;
    using ProcessingTools.Layout.Processors.Contracts.Normalizers;

    public class GenericDocumentTaggerWithNormalizationController<TTagger> : ITaggerController
        where TTagger : IDocumentTagger
    {
        private readonly TTagger tagger;
        private readonly IDocumentNormalizer documentNormalizer;

        public GenericDocumentTaggerWithNormalizationController(TTagger tagger, IDocumentNormalizer documentNormalizer)
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

        public async Task<object> Run(IDocument document, IProgramSettings settings)
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
