﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using Contracts.Commands;
    using Generics;
    using ProcessingTools.Attributes;
    using ProcessingTools.Processors.Contracts.Processors.Geo;

    [Description("Tag geo names.")]
    public class TagGeoNamesCommand : GenericDocumentTaggerCommand<IGeoNamesTagger>, ITagGeoNamesCommand
    {
        public TagGeoNamesCommand(IGeoNamesTagger tagger)
            : base(tagger)
        {
        }
    }
}