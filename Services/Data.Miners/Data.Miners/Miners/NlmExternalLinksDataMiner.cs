﻿namespace ProcessingTools.Data.Miners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Contracts;
    using Models;
    using ProcessingTools.Extensions;
    using ProcessingTools.Extensions.Linq;
    using ProcessingTools.Nlm.Publishing.Types;

    public class NlmExternalLinksDataMiner : INlmExternalLinksDataMiner
    {
        private const string UriPatternSuffix = @"(?=(?:&gt;|>)?\]?[,;\.]*[\)\s]+(?:[\.;\)]+\s)?|[\.;\)]+$|$)";
        private const string IPAddressPattern = @"\b(?:(?:(?:0?0?[0-9]|0?[0-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\.){3,3}(?:0?0?[0-9]|0?[0-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))\b(?:(?::\d+)?/)?";

        private const string HttpPattern = @"(?i)https?://(?:www)?\S+?" + UriPatternSuffix + @"|" +
            @"(?i)(?<!://)www\S+?" + UriPatternSuffix + @"|" +
            IPAddressPattern + @"(?:\D\S*)" + UriPatternSuffix + @"|" +
            @"[A-Za-z0-9][A-Za-z0-9@~&:\.\-_]*\.(?:com|org|net|edu)\b\S*?" + UriPatternSuffix;

        private const string FtpPattern = @"(?i)s?ftp://(?:www)?\S+?" + UriPatternSuffix;

        private const string DoiPattern = @"(?i)(?<=\bdoi\W{0,3})\d+\S+" + UriPatternSuffix + @"|" +
            @"10\.\d{4,5}/\S+" + UriPatternSuffix;

        private const string PmidPattern = @"(?i)(?<=\bpmid\W?)\d+";

        private const string PmcidPattern = @"(?i)\bpmc\W?\d+|(?i)(?<=\bpmcid\W?)\d+";

        public async Task<IQueryable<NlmExternalLink>> Mine(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            var internalMiner = new InternalMiner(content);

            var doiItems = await internalMiner.MineDoi().ToArrayAsync();
            var uriItems = await internalMiner.MineUri().ToArrayAsync();
            var ftpItems = await internalMiner.MineFtp().ToArrayAsync();
            var pmidItems = await internalMiner.MinePmid().ToArrayAsync();
            var pmcidItems = await internalMiner.MinePmcid().ToArrayAsync();

            var items = new List<NlmExternalLink>();
            items.AddRange(pmcidItems);
            items.AddRange(pmidItems);
            items.AddRange(ftpItems);
            items.AddRange(uriItems);
            items.AddRange(doiItems);

            var result = new HashSet<NlmExternalLink>(items);
            return result.AsQueryable();
        }

        private class InternalMiner
        {
            private string content;

            public InternalMiner(string content)
            {
                this.content = content;
            }

            public IEnumerable<NlmExternalLink> MineUri()
            {
                var matches = this.content.GetMatches(new Regex(HttpPattern));
                return matches.Select(item =>
                {
                    const string MatchDoiPrefixPattern = @"\A(?<prefix>https?://(?:dx\.)?doi.org/)(?<value>10\.\d{4,5}/.+)\Z";

                    var matchDoiPrefix = Regex.Match(item, MatchDoiPrefixPattern);
                    if (matchDoiPrefix.Success)
                    {
                        return new NlmExternalLink
                        {
                            Content = item,
                            Href = matchDoiPrefix.Groups["value"].Value,
                            Type = ExternalLinkType.Doi
                        };
                    }
                    else
                    {
                        return new NlmExternalLink
                        {
                            Content = item,
                            Href = item,
                            Type = ExternalLinkType.Uri
                        };
                    }
                });
            }

            public IEnumerable<NlmExternalLink> MineFtp()
            {
                var matches = this.content.GetMatches(new Regex(FtpPattern));
                return matches.Select(item => new NlmExternalLink
                {
                    Content = item,
                    Href = item,
                    Type = ExternalLinkType.Ftp
                });
            }

            public IEnumerable<NlmExternalLink> MineDoi()
            {
                var matches = this.content.GetMatches(new Regex(DoiPattern));
                return matches.Select(item => new NlmExternalLink
                {
                    Content = item,
                    Href = item,
                    Type = ExternalLinkType.Doi
                });
            }

            public IEnumerable<NlmExternalLink> MinePmid()
            {
                var matches = this.content.GetMatches(new Regex(PmidPattern));
                return matches.Select(item => new NlmExternalLink
                {
                    Content = item,
                    Href = item,
                    Type = ExternalLinkType.Pmid
                });
            }

            public IEnumerable<NlmExternalLink> MinePmcid()
            {
                var matches = this.content.GetMatches(new Regex(PmcidPattern));
                return matches.Select(item => new NlmExternalLink
                {
                    Content = item,
                    Href = item,
                    Type = ExternalLinkType.Pmcid
                });
            }
        }
    }
}