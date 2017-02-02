﻿namespace ProcessingTools.Tagger.Core.Commands
{
    using Contracts.Commands;
    using Generics;
    using ProcessingTools.Layout.Processors.Contracts.Normalizers;
    using Processors.Contracts.Processors.Bio.Taxonomy.Taggers;

    public class TagLowerTaxaInItalicCommand : GenericDocumentTaggerWithNormalizationCommand<ILowerTaxaInItalicTagger>, ITagLowerTaxaCommand
    {
        public TagLowerTaxaInItalicCommand(ILowerTaxaInItalicTagger tagger, IDocumentNormalizer documentNormalizer)
            : base(tagger, documentNormalizer)
        {
        }
    }
}
