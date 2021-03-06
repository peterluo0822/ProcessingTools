﻿namespace ProcessingTools.DataResources.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Common.Exceptions;
    using ProcessingTools.Constants;
    using ProcessingTools.DataResources.Data.Entity.Contracts;
    using ProcessingTools.DataResources.Data.Entity.Models;
    using ProcessingTools.DataResources.Services.Data.Contracts;
    using ProcessingTools.DataResources.Services.Data.Models;
    using ProcessingTools.DataResources.Services.Data.Models.Contracts;

    public class ContentTypesDataService : IContentTypesDataService
    {
        private readonly IResourcesDbContextProvider contextProvider;

        public ContentTypesDataService(IResourcesDbContextProvider contextProvider)
        {
            this.contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        }

        public async Task<object> Add(IContentTypeCreateServiceModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            int result = 0;

            using (var db = this.contextProvider.Create())
            {
                var entity = new ContentType
                {
                    Name = model.Name
                };

                db.ContentTypes.Add(entity);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IEnumerable<IContentTypeServiceModel>> All()
        {
            IEnumerable<IContentTypeServiceModel> result = null;

            using (var db = this.contextProvider.Create())
            {
                var query = db.ContentTypes
                    .OrderBy(e => e.Name)
                    .Select(e => new ContentTypeServiceModel
                    {
                        Id = e.Id,
                        Name = e.Name
                    });

                result = await query.ToListAsync();
            }

            return result;
        }

        public async Task<IEnumerable<IContentTypeServiceModel>> All(int pageNumber, int numberOfItemsPerPage)
        {
            if (pageNumber < 0)
            {
                throw new InvalidPageNumberException();
            }

            if (numberOfItemsPerPage < 1 || numberOfItemsPerPage > PagingConstants.MaximalItemsPerPageAllowed)
            {
                throw new InvalidItemsPerPageException();
            }

            IEnumerable<IContentTypeServiceModel> result = null;

            using (var db = this.contextProvider.Create())
            {
                var query = db.ContentTypes
                    .OrderBy(e => e.Name)
                    .Skip(pageNumber * numberOfItemsPerPage)
                    .Take(numberOfItemsPerPage)
                    .Select(e => new ContentTypeServiceModel
                    {
                        Id = e.Id,
                        Name = e.Name
                    });

                result = await query.ToListAsync();
            }

            return result;
        }

        public async Task<long> Count()
        {
            long result = 0L;
            using (var db = this.contextProvider.Create())
            {
                result = await db.ContentTypes.LongCountAsync();
            }

            return result;
        }

        public async Task<object> Delete(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            int result = 0;
            using (var db = this.contextProvider.Create())
            {
                var entity = db.ContentTypes.Find(id);
                db.ContentTypes.Remove(entity);
                result = await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IContentTypeDetailsServiceModel> GetDetails(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            IContentTypeDetailsServiceModel result = null;
            using (var db = this.contextProvider.Create())
            {
                var query = db.ContentTypes
                    .Where(e => e.Id.ToString() == id.ToString())
                    .Select(e => new ContentTypeDetailsServiceModel
                    {
                        Id = e.Id,
                        Name = e.Name
                    });

                result = await query.FirstOrDefaultAsync();
            }

            return result;
        }

        public async Task<object> Update(IContentTypeUpdateServiceModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            int result = 0;
            using (var db = this.contextProvider.Create())
            {
                var entity = new ContentType
                {
                    Id = model.Id,
                    Name = model.Name
                };

                db.Entry(entity).State = EntityState.Modified;
                result = await db.SaveChangesAsync();
            }

            return result;
        }
    }
}
