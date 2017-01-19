﻿namespace ProcessingTools.Tagger.Commands
{
    using Contracts.Commands;
    using Generics;

    using ProcessingTools.Attributes;
    using ProcessingTools.Processors.Contracts.Bio.EnvironmentTerms;

    [Description("Tag envo terms using local database.")]
    public class TagEnvironmentTermsController : GenericDocumentTaggerController<IEnvironmentTermsTagger>, ITagEnvironmentTermsController
    {
        public TagEnvironmentTermsController(IEnvironmentTermsTagger tagger)
            : base(tagger)
        {
        }
    }
}
