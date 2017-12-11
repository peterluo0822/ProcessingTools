﻿namespace ProcessingTools.Data.Miners.Contracts.Miners.Bio
{
    using ProcessingTools.Contracts.Data.Miners;
    using ProcessingTools.Contracts.Models.Services.Data.Bio.Biorepositories;

    public interface IBiorepositoriesInstitutionsDataMiner : IDataMiner<string, IInstitution>
    {
    }
}
