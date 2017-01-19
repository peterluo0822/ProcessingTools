﻿namespace ProcessingTools.Tagger.Commands
{
    using Contracts.Commands;
    using Generics;

    using ProcessingTools.Attributes;
    using ProcessingTools.Processors.Contracts.Geo;

    [Description("Tag geo epithets.")]
    public class TagGeoEpithetsController : GenericDocumentTaggerController<IGeoEpithetsTagger>, ITagGeoEpithetsController
    {
        public TagGeoEpithetsController(IGeoEpithetsTagger tagger)
            : base(tagger)
        {
        }
    }
}
