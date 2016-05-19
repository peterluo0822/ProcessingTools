﻿namespace ProcessingTools.Documents.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Contracts;
    using Models;

    using ProcessingTools.Documents.Data.Models;
    using ProcessingTools.Documents.Data.Repositories.Contracts;
    using ProcessingTools.Services.Common.Factories;

    public class PublishersDataService : GenericRepositoryProviderDataServiceFactory<Publisher, PublisherServiceModel>, IPublishersDataService
    {
        public PublishersDataService(IDocumentsRepositoryProvider<Publisher> repository)
            : base(repository)
        {
        }

        protected override Expression<Func<Publisher, IEnumerable<PublisherServiceModel>>> MapDbModelToServiceModel => p => new PublisherServiceModel[]
        {
            new PublisherServiceModel
            {
                AbbreviatedName = p.AbbreviatedName,
                CreatedByUserId = p.CreatedByUserId,
                DateCreated = p.DateCreated,
                DateModified = p.DateModified,
                Id = p.Id,
                ModifiedByUserId = p.ModifiedByUserId,
                Name = p.Name,
                Journals = p.Journals.Select(j => new JournalServiceModel
                {
                    AbbreviatedName = j.AbbreviatedName,
                    CreatedByUserId = j.CreatedByUserId,
                    DateCreated = j.DateCreated,
                    DateModified = j.DateModified,
                    ElectronicIssn = j.ElectronicIssn,
                    Id = j.Id,
                    JournalId = j.JournalId,
                    ModifiedByUserId = j.ModifiedByUserId,
                    Name = j.Name,
                    PrintIssn = j.PrintIssn,
                    PublisherId = j.PublisherId
                })
            }
        };

        protected override Expression<Func<PublisherServiceModel, IEnumerable<Publisher>>> MapServiceModelToDbModel => p => new Publisher[]
        {
            new Publisher
            {
                Name = p.Name,
                ModifiedByUserId = p.ModifiedByUserId,
                Id = p.Id,
                DateModified = p.DateModified,
                DateCreated = p.DateCreated,
                CreatedByUserId = p.CreatedByUserId,
                AbbreviatedName = p.AbbreviatedName
            }
        };

        protected override Expression<Func<Publisher, object>> SortExpression => p => p.Name;
    }
}