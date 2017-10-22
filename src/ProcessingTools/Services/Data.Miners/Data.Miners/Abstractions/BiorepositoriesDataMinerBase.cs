﻿namespace ProcessingTools.Data.Miners.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Constants;
    using ProcessingTools.Services.Contracts.Data.Bio.Biorepositories;

    public abstract class BiorepositoriesDataMinerBase<T>
        where T : class
    {
        protected async Task GetMatches(IBiorepositoriesDataService<T> service, ICollection<T> matches, Func<T, bool> filter)
        {
            int n = PaginationConstants.MaximalItemsPerPageAllowed;
            for (int i = 0; ; i += n)
            {
                var data = await service.GetAsync(i, n).ConfigureAwait(false);

                if (data.Length < 1)
                {
                    break;
                }

                data.Where(filter).ToList().ForEach(m => matches.Add(m));
            }
        }
    }
}
