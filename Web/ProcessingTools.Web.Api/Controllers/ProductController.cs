﻿namespace ProcessingTools.Web.Api.Controllers
{
    using Factories;
    using Models.Products;
    using Services.Data.Contracts;
    using Services.Data.Models.Contracts;

    public class ProductController : GenericDataServiceControllerFactory<IProductServiceModel, ProductRequestModel, ProductResponseModel>
    {
        public ProductController(IProductsDataService service)
            : base(service)
        {
        }
    }
}