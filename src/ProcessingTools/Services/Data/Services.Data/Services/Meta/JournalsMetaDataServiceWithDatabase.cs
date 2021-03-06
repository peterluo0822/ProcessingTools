﻿namespace ProcessingTools.Services.Data.Services.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Common.Extensions.Linq;
    using ProcessingTools.Contracts.Data.Documents.Repositories;
    using ProcessingTools.Contracts.Models.Documents;
    using ProcessingTools.Services.Data.Contracts.Meta;
    using ProcessingTools.Services.Data.Models.Meta;

    public class JournalsMetaDataServiceWithDatabase : IJournalsMetaDataService
    {
        private readonly IJournalMetaRepository repository;

        public JournalsMetaDataServiceWithDatabase(IJournalMetaRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<IJournalMeta>> GetAllJournalsMeta()
        {
            var query = await this.repository.Find(j => true);
            var result = await query.Select(j => new JournalMeta
            {
                AbbreviatedJournalTitle = j.AbbreviatedJournalTitle,
                ArchiveNamePattern = j.ArchiveNamePattern,
                FileNamePattern = j.FileNamePattern,
                IssnEPub = j.IssnEPub,
                IssnPPub = j.IssnPPub,
                JournalId = j.JournalId,
                JournalTitle = j.JournalTitle,
                PublisherName = j.PublisherName
            })
            .ToListAsync();

            return result;
        }
    }
}
