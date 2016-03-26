﻿namespace ProcessingTools.Web.Api.Models.Products
{
    using Contracts.Mapping;
    using Services.Data.Models.Contracts;

    public class ProductResponseModel : IMapFrom<IProductServiceModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}