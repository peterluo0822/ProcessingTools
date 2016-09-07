﻿namespace ProcessingTools.Data.Miners.Tests.RegressionTests.Services.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProcessingTools.Services.Data.Contracts;
    using ProcessingTools.Services.Data.Models;

    public class FakeInstitutionsDataService : IInstitutionsDataService
    {
        private const int NumberOfDataItems = 500;

        public FakeInstitutionsDataService()
        {
            this.Items = new HashSet<InstitutionServiceModel>();
            for (int i = 0; i < NumberOfDataItems; ++i)
            {
                this.Items.Add(new InstitutionServiceModel
                {
                    Id = i,
                    Name = $"Institution {i}"
                });
            }
        }

        public HashSet<InstitutionServiceModel> Items { get; set; }

        public Task<object> Add(params InstitutionServiceModel[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<InstitutionServiceModel>> All()
        {
            return Task.FromResult(this.Items.AsQueryable());
        }

        public Task<object> Delete(params object[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<object> Delete(params InstitutionServiceModel[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<InstitutionServiceModel>> Get(params object[] ids)
        {
            var result = ids.Select(id => this.Items.FirstOrDefault(p => p.Id == (int)id)).AsQueryable();

            return Task.FromResult(result);
        }

        public Task<IQueryable<InstitutionServiceModel>> Query(int skip, int take)
        {
            return Task.FromResult(this.Items
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(take)
                .AsQueryable());
        }

        public Task<object> Update(params InstitutionServiceModel[] entities)
        {
            throw new NotImplementedException();
        }
    }
}