﻿/*
 * Deviation:
 * -36.5806 149.3153 ±100 m
 * 24 km W
 */

namespace ProcessingTools.Data.Miners.Miners.Geo
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using ProcessingTools.Data.Miners.Contracts.Miners.Geo;
    using ProcessingTools.Extensions;

    public class GeographicDeviationsDataMiner : IGeographicDeviationsDataMiner
    {
        private const string DistancePattern = @"(\d+(?:[,\.]\d+)?(?:\s*[\(\)\[\]\{\}×\*])?\s*)+?k?m";

        public async Task<string[]> MineAsync(string context)
        {
            if (string.IsNullOrWhiteSpace(context))
            {
                throw new ArgumentNullException(nameof(context));
            }

            const string Pattern = DistancePattern + @"\W{0,4}(?:[NSEW][NSEW\s\.-]{0,5}(?!\w)|(?i)(?:east|west|south|notrh)+)";

            var data = await context.GetMatchesAsync(new Regex(Pattern)).ConfigureAwait(false);
            return data.Distinct().ToArray();
        }
    }
}