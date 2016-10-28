﻿namespace ProcessingTools.Processors.Generics
{
    using System;
    using System.Threading.Tasks;

    using Contracts.Providers;

    using ProcessingTools.Constants.Schema;
    using ProcessingTools.Contracts;
    using ProcessingTools.Data.Miners.Common.Contracts;
    using ProcessingTools.Harvesters.Contracts.Content;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;

    public class GenericStringMinerTagger<TMiner, TTagModelProvider> : IDocumentTagger
        where TMiner : IStringDataMiner
        where TTagModelProvider : IXmlTagModelProvider
    {
        private readonly TMiner miner;
        private readonly ITextContentHarvester contentHarvester;
        private readonly IStringTagger tagger;
        private readonly TTagModelProvider tagModelProvider;

        public GenericStringMinerTagger(
            TMiner miner,
            ITextContentHarvester contentHarvester,
            IStringTagger tagger,
            TTagModelProvider tagModelProvider)
        {
            if (miner == null)
            {
                throw new ArgumentNullException(nameof(miner));
            }

            if (contentHarvester == null)
            {
                throw new ArgumentNullException(nameof(contentHarvester));
            }

            if (tagger == null)
            {
                throw new ArgumentNullException(nameof(tagger));
            }

            if (tagModelProvider == null)
            {
                throw new ArgumentNullException(nameof(tagModelProvider));
            }

            this.miner = miner;
            this.contentHarvester = contentHarvester;
            this.tagger = tagger;
            this.tagModelProvider = tagModelProvider;
        }

        public async Task<object> Tag(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var tagModel = this.tagModelProvider.TagModel(document.XmlDocument);

            var textContent = await this.contentHarvester.Harvest(document.XmlDocument.DocumentElement);
            var data = await this.miner.Mine(textContent);

            await this.tagger.Tag(document, data, tagModel, XPathConstants.SelectContentNodesXPath);

            return true;
        }
    }
}