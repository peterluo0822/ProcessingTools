﻿namespace ProcessingTools.Processors.Processors.Bio.Codes
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Processors.Processors.Bio.Codes;
    using ProcessingTools.Contracts.Models.Services.Data.Bio.Biorepositories;
    using ProcessingTools.Data.Miners.Contracts.Miners.Bio;
    using ProcessingTools.Harvesters.Contracts.Harvesters.Content;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;
    using ProcessingTools.Processors.Models.Bio.Codes;
    using ProcessingTools.Processors.Models.Layout;

    public class InstitutionalCodesTagger : IInstitutionalCodesTagger
    {
        private const string XPath = "./*";
        private readonly ITextContentHarvester contentHarvester;
        private readonly IBiorepositoriesInstitutionsDataMiner miner;
        private readonly ISimpleXmlSerializableObjectTagger<BiorepositoriesInstitutionalCodeSerializableModel> institutionalCodesTagger;
        private readonly ISimpleXmlSerializableObjectTagger<BiorepositoriesInstitutionSerializableModel> institutionsTagger;

        public InstitutionalCodesTagger(
            ITextContentHarvester contentHarvester,
            IBiorepositoriesInstitutionsDataMiner miner,
            ISimpleXmlSerializableObjectTagger<BiorepositoriesInstitutionalCodeSerializableModel> institutionalCodesTagger,
            ISimpleXmlSerializableObjectTagger<BiorepositoriesInstitutionSerializableModel> institutionsTagger)
        {
            this.contentHarvester = contentHarvester ?? throw new ArgumentNullException(nameof(contentHarvester));
            this.miner = miner ?? throw new ArgumentNullException(nameof(miner));
            this.institutionalCodesTagger = institutionalCodesTagger ?? throw new ArgumentNullException(nameof(institutionalCodesTagger));
            this.institutionsTagger = institutionsTagger ?? throw new ArgumentNullException(nameof(institutionsTagger));
        }

        public async Task<object> TagAsync(IDocument context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var textContent = await this.contentHarvester.HarvestAsync(context.XmlDocument.DocumentElement).ConfigureAwait(false);
            var data = (await this.miner.MineAsync(textContent).ConfigureAwait(false))
                .ToArray();

            await this.TagInstitutionalCodes(context, data).ConfigureAwait(false);
            await this.TagInstitutions(context, data).ConfigureAwait(false);

            return true;
        }

        private async Task TagInstitutionalCodes(IDocument document, IInstitution[] data)
        {
            var institutionalCodes = data.Select(i => new BiorepositoriesInstitutionalCodeSerializableModel
            {
                Description = i.Name,
                Url = i.Url,
                Value = i.Code
            });

            var settings = new ContentTaggerSettings
            {
                CaseSensitive = true,
                MinimalTextSelect = true
            };

            await this.institutionalCodesTagger.Tag(document.XmlDocument.DocumentElement, document.NamespaceManager, institutionalCodes, XPath, settings).ConfigureAwait(false);
        }

        private async Task TagInstitutions(IDocument document, IInstitution[] data)
        {
            var institutions = data.Select(i => new BiorepositoriesInstitutionSerializableModel
            {
                Url = i.Url,
                Value = i.Name
            });

            var settings = new ContentTaggerSettings
            {
                CaseSensitive = true,
                MinimalTextSelect = true
            };

            await this.institutionsTagger.Tag(document.XmlDocument.DocumentElement, document.NamespaceManager, institutions, XPath, settings).ConfigureAwait(false);
        }
    }
}
