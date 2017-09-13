﻿namespace ProcessingTools.Processors.Processors.Bio.Codes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Data.Miners.Contracts.Miners.Bio;
    using ProcessingTools.Data.Miners.Contracts.Models.Bio;
    using ProcessingTools.Harvesters.Contracts.Harvesters.Content;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;
    using ProcessingTools.Layout.Processors.Models.Taggers;
    using ProcessingTools.Processors.Contracts.Processors.Bio.Codes;
    using ProcessingTools.Processors.Models.Bio.Codes;

    public class CollectionCodesTagger : ICollectionCodesTagger
    {
        private const string XPath = "./*";
        private readonly ITextContentHarvester contentHarvester;
        private readonly IBiorepositoriesCollectionsDataMiner miner;
        private readonly ISimpleXmlSerializableObjectTagger<BiorepositoriesCollectionCodeSerializableModel> collectionCodesTagger;
        private readonly ISimpleXmlSerializableObjectTagger<BiorepositoriesCollectionSerializableModel> collectionsTagger;

        public CollectionCodesTagger(
            ITextContentHarvester contentHarvester,
            IBiorepositoriesCollectionsDataMiner miner,
            ISimpleXmlSerializableObjectTagger<BiorepositoriesCollectionCodeSerializableModel> collectionCodesTagger,
            ISimpleXmlSerializableObjectTagger<BiorepositoriesCollectionSerializableModel> collectionsTagger)
        {
            this.contentHarvester = contentHarvester ?? throw new ArgumentNullException(nameof(contentHarvester));
            this.miner = miner ?? throw new ArgumentNullException(nameof(miner));
            this.collectionCodesTagger = collectionCodesTagger ?? throw new ArgumentNullException(nameof(collectionCodesTagger));
            this.collectionsTagger = collectionsTagger ?? throw new ArgumentNullException(nameof(collectionsTagger));
        }

        public async Task<object> TagAsync(IDocument context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var textContent = await this.contentHarvester.Harvest(context.XmlDocument.DocumentElement).ConfigureAwait(false);
            var data = (await this.miner.Mine(textContent).ConfigureAwait(false))
                .ToArray();

            await this.TagCollectionCodes(context, data).ConfigureAwait(false);
            await this.TagCollections(context, data).ConfigureAwait(false);

            return true;
        }

        private async Task TagCollectionCodes(
            IDocument document,
            IEnumerable<IBiorepositoriesCollection> data)
        {
            var collectionCodes = data.Select(c => new BiorepositoriesCollectionCodeSerializableModel
            {
                Url = c.Url,
                Value = c.CollectionCode,
                XLinkTitle = c.CollectionName
            });

            var settings = new ContentTaggerSettings
            {
                CaseSensitive = true,
                MinimalTextSelect = true
            };

            await this.collectionCodesTagger.Tag(document.XmlDocument.DocumentElement, document.NamespaceManager, collectionCodes, XPath, settings).ConfigureAwait(false);
        }

        private async Task TagCollections(
            IDocument document,
            IEnumerable<IBiorepositoriesCollection> data)
        {
            var collections = data.Select(c => new BiorepositoriesCollectionSerializableModel
            {
                Url = c.Url,
                Value = c.CollectionName
            });

            var settings = new ContentTaggerSettings
            {
                CaseSensitive = true,
                MinimalTextSelect = true
            };

            await this.collectionsTagger.Tag(document.XmlDocument.DocumentElement, document.NamespaceManager, collections, XPath, settings).ConfigureAwait(false);
        }
    }
}
