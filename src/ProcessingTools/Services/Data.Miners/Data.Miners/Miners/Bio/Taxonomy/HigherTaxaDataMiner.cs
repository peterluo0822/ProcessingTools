﻿namespace ProcessingTools.Data.Miners.Miners.Bio.Taxonomy
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using ProcessingTools.Common.Extensions;
    using ProcessingTools.Common.Extensions.Linq;
    using ProcessingTools.Constants;
    using ProcessingTools.Data.Miners.Contracts.Miners.Bio.Taxonomy;

    public class HigherTaxaDataMiner : IHigherTaxaDataMiner
    {
        private readonly Regex matchHigherTaxa = new Regex(TaxaRegexPatterns.HigherTaxaMatchPattern);

        public async Task<IEnumerable<string>> MineAsync(string context, IEnumerable<string> seed, IEnumerable<string> stopWords)
        {
            if (string.IsNullOrWhiteSpace(context))
            {
                return new string[] { };
            }

            var words = await context.ExtractWordsFromText()
                .DistinctWithStopWords(stopWords)
                .ToArrayAsync()
                .ConfigureAwait(false);

            var result = await words.Where(w => this.matchHigherTaxa.IsMatch(w))
                .Union(words.MatchWithSeedWords(seed))
                .ToArrayAsync()
                .ConfigureAwait(false);

            return new HashSet<string>(result);
        }
    }
}
