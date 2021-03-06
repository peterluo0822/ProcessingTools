﻿namespace ProcessingTools.Data.Miners.Miners.Bio
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using ProcessingTools.Bio.Biorepositories.Services.Data.Contracts;
    using ProcessingTools.Bio.Biorepositories.Services.Data.Models;
    using ProcessingTools.Data.Miners.Abstractions;
    using ProcessingTools.Data.Miners.Contracts.Miners.Bio;
    using ProcessingTools.Data.Miners.Contracts.Models.Bio;
    using ProcessingTools.Data.Miners.Models.Bio;

    public class BiorepositoriesInstitutionsDataMiner : BiorepositoriesDataMinerBase<BiorepositoriesInstitution, BiorepositoriesInstitutionServiceModel>, IBiorepositoriesInstitutionsDataMiner
    {
        private readonly IBiorepositoriesInstitutionsDataService service;

        public BiorepositoriesInstitutionsDataMiner(IBiorepositoriesInstitutionsDataService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this.service = service;
        }

        protected override Func<BiorepositoriesInstitutionServiceModel, BiorepositoriesInstitution> Project => x => new BiorepositoriesInstitution
        {
            InstitutionalCode = x.InstitutionalCode,
            NameOfInstitution = x.NameOfInstitution,
            Url = x.Url
        };

        public async Task<IEnumerable<IBiorepositoriesInstitution>> Mine(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            Func<BiorepositoriesInstitutionServiceModel, bool> filter = x => Regex.IsMatch(content, Regex.Escape(x.InstitutionalCode) + "|" + Regex.Escape(x.NameOfInstitution));

            var matches = new List<BiorepositoriesInstitution>();

            await this.GetMatches(this.service, matches, filter);

            var result = new HashSet<BiorepositoriesInstitution>(matches);
            return result;
        }
    }
}
