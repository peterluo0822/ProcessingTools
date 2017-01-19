﻿namespace ProcessingTools.Tagger.Commands
{
    using Contracts.Commands;
    using Generics;

    using ProcessingTools.Attributes;
    using ProcessingTools.Processors.Contracts.Dates;

    [Description("Tag dates.")]
    public class TagDatesController : GenericDocumentTaggerController<IDatesTagger>, ITagDatesController
    {
        public TagDatesController(IDatesTagger tagger)
            : base(tagger)
        {
        }
    }
}
