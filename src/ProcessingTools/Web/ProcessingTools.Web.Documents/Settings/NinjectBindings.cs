﻿namespace ProcessingTools.Web.Documents.Settings
{
    using System;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Extensions.Factory;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using ProcessingTools.Contracts;
    using ProcessingTools.Services.Data.Services.Files;
    using ProcessingTools.Tagger.Commands.Contracts;
    using ProcessingTools.Tagger.Commands.Contracts.Commands;

    /// <summary>
    /// NinjectModule to bind other infrastructure objects.
    /// </summary>
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind(b =>
            {
                b.FromThisAssembly()
                    .SelectAllClasses()
                    .BindDefaultInterface();

                b.FromAssembliesMatching("ProcessingTools.Web.*")
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind(b =>
            {
                b.From(typeof(ProcessingTools.Tagger.Commands.Contracts.Commands.ITaggerCommand).Assembly)
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind(typeof(ProcessingTools.Data.Contracts.Repositories.IGenericRepositoryProvider<>))
                .To(typeof(ProcessingTools.Data.Common.Repositories.RepositoryProvider<>));

            this.Bind(b =>
            {
                b.From(typeof(ProcessingTools.Net.NetConnector).Assembly)
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind(b =>
            {
                b.From(ProcessingTools.FileSystem.Assembly.Assembly.GetType().Assembly)
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind(b =>
            {
                b.From(ProcessingTools.Geo.Assembly.Assembly.GetType().Assembly)
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind<ProcessingTools.Geo.Contracts.Factories.ICoordinatesFactory>()
                .ToFactory()
                .InSingletonScope();

            this.Bind(b =>
            {
                b.From(ProcessingTools.Xml.Assembly.Assembly.GetType().Assembly)
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind(b =>
            {
                b.From(ProcessingTools.Processors.Assembly.Assembly.GetType().Assembly)
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind(b =>
            {
                b.From(ProcessingTools.Special.Processors.Assembly.Assembly.GetType().Assembly)
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind(b =>
            {
                b.From(ProcessingTools.Serialization.Assembly.Assembly.GetType().Assembly)
                    .SelectAllClasses()
                    .BindDefaultInterface();
            });

            this.Bind<ProcessingTools.Services.Contracts.Data.Files.IStreamingFilesDataService>()
                .To<StreamingSystemFilesDataService>();

            this.Bind<ProcessingTools.Contracts.IDocumentFactory>()
                .To<ProcessingTools.Common.TaxPubDocumentFactory>()
                .InSingletonScope();

            this.Bind<Func<Type, ITaggerCommand>>()
                .ToMethod(context => t => context.Kernel.Get(t) as ITaggerCommand)
                .InSingletonScope();

            this.Bind<IFactory<ICommandSettings>>()
                .ToFactory()
                .InSingletonScope();

            this.Bind<ProcessingTools.Contracts.IReporter>()
                .To<ProcessingTools.Reporters.LogReporter>();

            this.Bind<ProcessingTools.Contracts.ILogger>()
                .To<ProcessingTools.Loggers.Loggers.Log4NetLogger>()
                .InSingletonScope();

            this.Bind<ProcessingTools.Contracts.IDateTimeProvider>()
                .To<ProcessingTools.Services.Providers.DateTimeProvider>()
                .InSingletonScope();

            this.Bind<Func<Type, ProcessingTools.Processors.Contracts.Strategies.Bio.Taxonomy.IParseLowerTaxaStrategy>>()
                .ToMethod(context =>
                {
                    return t => context.Kernel.Get(t) as ProcessingTools.Processors.Contracts.Strategies.Bio.Taxonomy.IParseLowerTaxaStrategy;
                });

            this.Bind<ProcessingTools.Processors.Contracts.Imaging.IQRCodeEncoder>()
                .To<ProcessingTools.Imaging.Processors.QRCodeEncoder>()
                .InRequestScope();

            this.Bind<ProcessingTools.Processors.Contracts.Imaging.IBarcodeEncoder>()
                .To<ProcessingTools.Imaging.Processors.BarcodeEncoder>()
                .InRequestScope();
        }
    }
}
