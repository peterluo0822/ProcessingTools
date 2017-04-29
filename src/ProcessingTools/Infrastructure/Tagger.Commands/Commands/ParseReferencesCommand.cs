﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using Contracts.Commands;
    using Generics;
    using ProcessingTools.Attributes;
    using ProcessingTools.Processors.Contracts.Processors.References;

    [Description("Parse references.")]
    public class ParseReferencesCommand : GenericXmlContextParserCommand<IReferencesParser>, IParseReferencesCommand
    {
        public ParseReferencesCommand(IReferencesParser parser)
            : base(parser)
        {
        }
    }
}