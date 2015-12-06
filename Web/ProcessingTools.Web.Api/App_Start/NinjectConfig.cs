﻿namespace ProcessingTools.Web.Api
{
    using System;
    using System.Web;

    using Bio.Taxonomy.Services.Data;
    using Bio.Taxonomy.Services.Data.Contracts;
    using Common.Providers;
    using Common.Providers.Contracts;
    using MediaType.Services.Data;
    using MediaType.Services.Data.Contracts;
    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectConfig
    {
        public static Action<IKernel> DependenciesRegistration => kernel =>
        {
            kernel.Bind<MediaType.Data.Contracts.IMediaTypesDbContext>().To<MediaType.Data.MediaTypesDbContext>();
            kernel.Bind(typeof(MediaType.Data.Repositories.IMediaTypesRepository<>)).To(typeof(MediaType.Data.Repositories.MediaTypesGenericRepository<>));
            ////kernel.Bind<IMediaTypeDataService>().To<MediaTypeDataServiceStaticDictionary>();
            ////kernel.Bind<IMediaTypeDataService>().To<MediaTypeDataServiceWindowsRegistry>();
            kernel.Bind<IMediaTypeDataService>().To<MediaTypeDataService>();

            kernel.Bind<IRandomProvider>().To<RandomProvider>();

            kernel.Bind<IAphiaTaxaClassificationDataService>().To<AphiaTaxaClassificationDataService>();

            kernel.Bind<ICatalogueOfLifeTaxaClassificationDataService>().To<CatalogueOfLifeTaxaClassificationDataService>();

            kernel.Bind<IGbifTaxaClassificationDataService>().To<GbifTaxaClassificationDataService>();
        };

        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            DependenciesRegistration(kernel);

            ////kernel.Bind(b => b
            ////    .From(Assemblies.MediaTypeDataServices)
            ////    .SelectAllClasses()
            ////    .BindDefaultInterface());

            ////kernel.Bind(b => b
            ////    .From(Assemblies.TaxonomicDataServices)
            ////    .SelectAllClasses()
            ////    .BindDefaultInterface());
        }
    }
}