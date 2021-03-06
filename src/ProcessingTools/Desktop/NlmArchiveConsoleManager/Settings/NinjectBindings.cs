﻿namespace ProcessingTools.NlmArchiveConsoleManager.Settings
{
    using System.Configuration;
    using System.Reflection;
    using Ninject.Extensions.Conventions;
    using Ninject.Extensions.Factory;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Modules;
    using ProcessingTools.Constants;
    using ProcessingTools.Constants.Configuration;
    using ProcessingTools.Interceptors;
    using ProcessingTools.Loggers.Loggers;
    using ProcessingTools.NlmArchiveConsoleManager.Contracts.Factories;
    using ProcessingTools.NlmArchiveConsoleManager.Core;
    using ProcessingTools.Services.Data.Services.Files;

    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind(b =>
            {
                b.From(Assembly.GetExecutingAssembly())
                .SelectAllClasses()
                .BindDefaultInterface();

                b.FromAssembliesMatching(
                    $"{nameof(ProcessingTools)}.*.{FileConstants.DllFileExtension}")
                .SelectAllClasses()
                .BindDefaultInterface();
            });

            this.Bind<ProcessingTools.Contracts.IDocumentFactory>()
                .To<ProcessingTools.DocumentProvider.Factories.TaxPubDocumentFactory>()
                .InSingletonScope();

            this.Bind<ProcessingTools.Contracts.ILogger>()
                .To<ConsoleLogger>()
                .InSingletonScope();

            this.Bind<ProcessingTools.Contracts.Files.IO.IXmlFileReader>()
                .To<ProcessingTools.FileSystem.IO.XmlFileReader>()
                .WhenInjectedInto<XmlFileContentDataService>();

            this.Bind<ProcessingTools.Contracts.Files.IO.IXmlFileWriter>()
                .To<ProcessingTools.FileSystem.IO.XmlFileWriter>()
                .WhenInjectedInto<XmlFileContentDataService>()
                .Intercept()
                .With<FileExistsRaiseWarningInterceptor>();

            this.Bind<ProcessingTools.Contracts.IDeserializer>()
                .To<ProcessingTools.Serialization.Serializers.DataContractJsonDeserializer>()
                .InSingletonScope();

            this.Bind<IProcessorFactory>()
                .ToFactory()
                .InSingletonScope();

            this.Bind<IModelFactory>()
                .ToFactory()
                .InSingletonScope();

            this.Bind<ProcessingTools.Contracts.IReporter>()
                .To<ProcessingTools.Reporters.LogReporter>()
                .InSingletonScope();

            var appSettingsReader = new AppSettingsReader();
            string journalMetaFilesDirectory = appSettingsReader.GetValue(AppSettingsKeys.JournalsJsonFilesDirectoryName, typeof(string)).ToString();
            string documentsMongoConnection = appSettingsReader.GetValue(AppSettingsKeys.DocumentsMongoConnection, typeof(string)).ToString();
            string documentsMongoDabaseName = appSettingsReader.GetValue(AppSettingsKeys.DocumentsMongoDabaseName, typeof(string)).ToString();

            this.Bind<ProcessingTools.Services.Data.Contracts.Meta.IJournalMetaDataService>()
                .To<ProcessingTools.Services.Data.Services.Meta.JournalMetaDataServiceWithFiles>();

            ////this.Bind<ProcessingTools.Services.Data.Contracts.Meta.IJournalsMetaDataService>()
            ////    .To<ProcessingTools.Services.Data.Services.Meta.JournalsMetaDataServiceWithFiles>()
            ////    .WhenInjectedInto<Engine>()
            ////    .WithConstructorArgument(
            ////        ParameterNames.JournalMetaFilesDirectoryName,
            ////        journalMetaFilesDirectory);

            ////this.Bind<ProcessingTools.Services.Data.Contracts.Meta.IJournalsMetaDataService>()
            ////    .To<ProcessingTools.Services.Data.Services.Meta.JournalsMetaDataServiceWithFiles>()
            ////    .WhenInjectedInto<HelpProvider>()
            ////    .WithConstructorArgument(
            ////        ParameterNames.JournalMetaFilesDirectoryName,
            ////        journalMetaFilesDirectory);

            this.Bind<ProcessingTools.Services.Data.Contracts.Meta.IJournalsMetaDataService>()
                .To<ProcessingTools.Services.Data.Services.Meta.JournalsMetaDataServiceWithDatabase>()
                .WhenInjectedInto<Engine>();

            this.Bind<ProcessingTools.Services.Data.Contracts.Meta.IJournalsMetaDataService>()
                .To<ProcessingTools.Services.Data.Services.Meta.JournalsMetaDataServiceWithDatabase>()
                .WhenInjectedInto<HelpProvider>();

            this.Bind<ProcessingTools.Contracts.Data.Documents.Repositories.IJournalMetaRepository>()
                .To<ProcessingTools.Documents.Data.Mongo.Repositories.MongoJournalMetaRepository>();

            this.Bind<ProcessingTools.Data.Common.Mongo.Contracts.IMongoDatabaseProvider>()
                .To<ProcessingTools.Data.Common.Mongo.MongoDatabaseProvider>()
                .WhenInjectedInto<ProcessingTools.Documents.Data.Mongo.Repositories.MongoJournalMetaRepository>()
                .InSingletonScope()
                .WithConstructorArgument(
                    ParameterNames.ConnectionString,
                    documentsMongoConnection)
                .WithConstructorArgument(
                    ParameterNames.DatabaseName,
                    documentsMongoDabaseName);
        }
    }
}
