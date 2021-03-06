﻿namespace ProcessingTools.Tagger.Commands.Commands
{
    using System.ComponentModel;
    using ProcessingTools.Processors.Contracts.Processors.Bio.Taxonomy.Parsers;
    using ProcessingTools.Tagger.Commands.Contracts.Commands;
    using ProcessingTools.Tagger.Commands.Generics;

    [Description("Parse lower taxa.")]
    public class ParseLowerTaxaCommand : GenericXmlContextParserCommand<ILowerTaxaParser>, IParseLowerTaxaCommand
    {
        public ParseLowerTaxaCommand(ILowerTaxaParser parser)
            : base(parser)
        {
        }
    }
}
