﻿namespace ProcessingTools.Geo.Data.Entity.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using AutoMapper;
    using ProcessingTools.Contracts.Data.Repositories.Geo;
    using ProcessingTools.Contracts.Filters.Geo;
    using ProcessingTools.Contracts.Models.Geo;
    using ProcessingTools.Contracts.Services;
    using ProcessingTools.Geo.Data.Entity.Abstractions.Repositories;
    using ProcessingTools.Geo.Data.Entity.Contracts.Repositories;
    using ProcessingTools.Geo.Data.Entity.Models;

    public class EntityCountriesRepository : AbstractGeoSynonymisableRepository<Country, ICountry, ICountriesFilter, CountrySynonym, ICountrySynonym, ICountrySynonymsFilter>, ICountriesRepository
    {
        private readonly IMapper mapper;

        public EntityCountriesRepository(IGeoRepository<Country> repository, IGeoRepository<CountrySynonym> synonymRepository, IEnvironment environment)
            : base(repository, synonymRepository, environment)
        {
            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<Country, Country>()
                    .ForMember(d => d.States, o => o.Ignore())
                    .ForMember(d => d.Provinces, o => o.Ignore())
                    .ForMember(d => d.Regions, o => o.Ignore())
                    .ForMember(d => d.Districts, o => o.Ignore())
                    .ForMember(d => d.Municipalities, o => o.Ignore())
                    .ForMember(d => d.Counties, o => o.Ignore())
                    .ForMember(d => d.Cities, o => o.Ignore())
                    .ForMember(d => d.Synonyms, o => o.Ignore())
                    .ForMember(d => d.CreatedBy, o => o.Ignore())
                    .ForMember(d => d.CreatedOn, o => o.Ignore());

                c.CreateMap<CountrySynonym, CountrySynonym>()
                    .ForMember(d => d.Country, o => o.Ignore())
                    .ForMember(d => d.CreatedBy, o => o.Ignore())
                    .ForMember(d => d.CreatedOn, o => o.Ignore());

                c.CreateMap<ICountry, Country>()
                    .ForMember(d => d.States, o => o.Ignore())
                    .ForMember(d => d.Provinces, o => o.Ignore())
                    .ForMember(d => d.Regions, o => o.Ignore())
                    .ForMember(d => d.Districts, o => o.Ignore())
                    .ForMember(d => d.Municipalities, o => o.Ignore())
                    .ForMember(d => d.Counties, o => o.Ignore())
                    .ForMember(d => d.Cities, o => o.Ignore())
                    .ForMember(d => d.Synonyms, o => o.Ignore())
                    .ForMember(d => d.CreatedBy, o => o.Ignore())
                    .ForMember(d => d.CreatedOn, o => o.Ignore())
                    .ForMember(d => d.ModifiedBy, o => o.Ignore())
                    .ForMember(d => d.ModifiedOn, o => o.Ignore());

                c.CreateMap<ICountrySynonym, CountrySynonym>()
                    .ForMember(d => d.Country, o => o.Ignore())
                    .ForMember(d => d.CountryId, o => o.ResolveUsing(x => x.ParentId))
                    .ForMember(d => d.CreatedBy, o => o.Ignore())
                    .ForMember(d => d.CreatedOn, o => o.Ignore())
                    .ForMember(d => d.ModifiedBy, o => o.Ignore())
                    .ForMember(d => d.ModifiedOn, o => o.Ignore());

                c.CreateMap<Country, ICountry>()
                    .ConstructUsing(m => new CountryModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        AbbreviatedName = m.AbbreviatedName,
                        CallingCode = m.CallingCode,
                        Iso639xCode = m.Iso639xCode,
                        LanguageCode = m.LanguageCode,
                        Continents = m.Continents
                            .Select(x => new ContinentModel
                            {
                                Id = x.Id,
                                Name = x.Name
                            })
                            .ToList<IContinent>(),
                        Synonyms = m.Synonyms
                            .Select(s => new CountrySynonymModel
                            {
                                Id = s.Id,
                                LanguageCode = s.LanguageCode,
                                Name = s.Name,
                                ParentId = m.Id
                            })
                            .ToList<ICountrySynonym>()
                    });

                c.CreateMap<CountrySynonym, ICountrySynonym>()
                    .ConstructUsing(s => new CountrySynonymModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        LanguageCode = s.LanguageCode,
                        ParentId = s.CountryId
                    });
            });

            this.mapper = mapperConfiguration.CreateMapper();
        }

        protected override IMapper Mapper => this.mapper;

        protected override IQueryable<Country> GetQuery(ICountriesFilter filter)
        {
            var query = this.Repository.Queryable()
                .Include(e => e.Continents)
                .Include(e => e.Synonyms);

            if (filter != null)
            {
                query = query.Where(
                    c =>
                         (!filter.Id.HasValue || c.Id == filter.Id) &&
                         (string.IsNullOrEmpty(filter.Name) || c.Name.ToLower().Contains(filter.Name.ToLower())) &&
                         (string.IsNullOrEmpty(filter.Synonym) || c.Synonyms.Any(s => s.Name.ToLower().Contains(filter.Synonym.ToLower()))));
            }

            return query;
        }
    }
}
