﻿namespace ProcessingTools.Processors.Processors.Bio.Taxonomy.Formatters
{
    using System;
    using System.Threading.Tasks;
    using Contracts.Factories.Bio;
    using Contracts.Processors.Bio.Taxonomy.Formatters;
    using ProcessingTools.Contracts;

    public class TaxonNamePartsRemover : ITaxonNamePartsRemover
    {
        private readonly IBioTaxonomyTransformersFactory transformersFactory;

        public TaxonNamePartsRemover(IBioTaxonomyTransformersFactory transformersFactory)
        {
            if (transformersFactory == null)
            {
                throw new ArgumentNullException(nameof(transformersFactory));
            }

            this.transformersFactory = transformersFactory;
        }

        public async Task<object> Format(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var content = await this.transformersFactory
                .GetRemoveTaxonNamePartsTransformer()
                .Transform(document.Xml);

            document.Xml = content;

            return true;
        }
    }
}
