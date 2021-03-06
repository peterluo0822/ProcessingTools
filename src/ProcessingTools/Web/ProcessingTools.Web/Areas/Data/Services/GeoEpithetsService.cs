﻿namespace ProcessingTools.Web.Areas.Data.Services
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using ProcessingTools.Common.Exceptions;
    using ProcessingTools.Constants;
    using ProcessingTools.Contracts.Models.Geo;
    using ProcessingTools.Contracts.Services.Data.Geo;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Models.ViewModels;
    using ProcessingTools.Web.Areas.Data.Controllers;
    using ProcessingTools.Web.Areas.Data.Models.GeoEpithets;
    using Strings = ProcessingTools.Web.Areas.Data.Resources.GeoEpithets.Views_Strings;

    public class GeoEpithetsService : IGeoEpithetsService
    {
        private readonly IGeoEpithetsDataService service;
        private readonly IMapper mapper;

        public GeoEpithetsService(IGeoEpithetsDataService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));

            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<IGeoEpithet, GeoEpithetViewModel>();
                c.CreateMap<GeoEpithetRequestModel, GeoEpithetViewModel>();
            });

            this.mapper = mapperConfiguration.CreateMapper();
        }

        public async Task<GeoEpithetsIndexPageViewModel> SelectAsync(int currentPage, int numberOfItemsPerPage)
        {
            if (currentPage < PagingConstants.MinimalPageNumber)
            {
                throw new InvalidPageNumberException();
            }

            if (numberOfItemsPerPage < PagingConstants.MinimalItemsPerPage || numberOfItemsPerPage > PagingConstants.MaximalItemsPerPageAllowed)
            {
                throw new InvalidItemsPerPageException();
            }

            long numberOfItems = await this.service.SelectCountAsync(null);
            var data = await this.service.SelectAsync(null, currentPage * numberOfItemsPerPage, numberOfItemsPerPage, nameof(IGeoName.Name), SortOrder.Ascending);
            var items = data.Select(this.mapper.Map<GeoEpithetViewModel>).ToArray();

            var model = new ListWithPagingViewModel<GeoEpithetViewModel>(GeoEpithetsController.IndexActionName, numberOfItems, numberOfItemsPerPage, currentPage, items);
            var viewModel = new GeoEpithetsIndexPageViewModel
            {
                Model = model,
                PageTitle = Strings.IndexPageTitle
            };
            return viewModel;
        }

        public async Task InsertAsync(GeoEpithetsRequestModel model)
        {
            await this.service.InsertAsync(model.ToArray());
        }

        public async Task UpdateAsync(GeoEpithetRequestModel model)
        {
            await this.service.UpdateAsync(model);
        }

        public async Task DeleteAsync(int id)
        {
            await this.service.DeleteAsync(ids: id);
        }
    }
}
