﻿namespace ProcessingTools.NlmArchiveConsoleManager.Settings
{
    using Core;
    using Ninject.Extensions.Conventions;
    using Ninject.Extensions.Factory;
    using Ninject.Extensions.Interception.Infrastructure.Language;
    using Ninject.Modules;
    using ProcessingTools.Constants;
    using ProcessingTools.Constants.Configuration;
    using ProcessingTools.Interceptors;
    using ProcessingTools.Loggers.Loggers;
    using ProcessingTools.NlmArchiveConsoleManager.Contracts.Factories;
    using ProcessingTools.Services.Data.Services.Files;
    using System.Configuration;
    using System.Reflection;

    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind(b =>
            {
                b.From(Assembly.GetExecutingAssembly())
                .SelectAllClasses()
                .BindDefaultInterface();
            });

            this.Bind(b =>
            {
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

            {
                var appSettingsReader = new AppSettingsReader();
                string journalMetaFilesDirectory = appSettingsReader.GetValue(AppSettingsKeys.JournalsJsonFilesDirectoryName, typeof(string)).ToString();

                this.Bind<ProcessingTools.Services.Data.Contracts.Meta.IJournalsMetaDataService>()
                    .To<ProcessingTools.Services.Data.Services.Meta.JournalsMetaDataService>()
                    .WhenInjectedInto<Engine>()
                    .WithConstructorArgument(
                        ParameterNames.JournalMetaFilesDirectoryName,
                        journalMetaFilesDirectory);

                this.Bind<ProcessingTools.Services.Data.Contracts.Meta.IJournalsMetaDataService>()
                    .To<ProcessingTools.Services.Data.Services.Meta.JournalsMetaDataService>()
                    .WhenInjectedInto<HelpProvider>()
                    .WithConstructorArgument(
                        ParameterNames.JournalMetaFilesDirectoryName,
                        journalMetaFilesDirectory);
            }
        }
    }
}
