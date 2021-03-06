﻿namespace ProcessingTools.Services.Data.Contracts.Meta
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts.Models.Documents;

    public interface IJournalsMetaDataService
    {
        Task<IEnumerable<IJournalMeta>> GetAllJournalsMeta();
    }
}
