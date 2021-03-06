﻿namespace ProcessingTools.NlmArchiveConsoleManager.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Core;
    using Contracts.Factories;
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Models.Documents;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Services.Data.Contracts.Meta;

    public class Engine : IEngine
    {
        private readonly IHelpProvider helpProvider;
        private readonly IJournalsMetaDataService journalsMetaService;
        private readonly ILogger logger;
        private readonly IProcessorFactory processorFactory;

        public Engine(
            IProcessorFactory processorFactory,
            IJournalsMetaDataService journalsMetaService,
            IHelpProvider helpProvider,
            ILogger logger)
        {
            this.processorFactory = processorFactory ?? throw new ArgumentNullException(nameof(processorFactory));
            this.journalsMetaService = journalsMetaService ?? throw new ArgumentNullException(nameof(journalsMetaService));
            this.helpProvider = helpProvider ?? throw new ArgumentNullException(nameof(helpProvider));
            this.logger = logger;
        }

        private Func<string, bool> FilterDoubleDashedOption => a => a.IndexOf("--") == 0;

        private Func<string, bool> FilterNonDashedOption => a => a.IndexOf("--") < 0;

        private Func<string, string> SelectDirectoryName => x =>
        {
            if (Directory.Exists(x))
            {
                return x;
            }
            else
            {
                string path = Path.GetDirectoryName(x);

                if (Directory.Exists(path))
                {
                    return path;
                }
                else
                {
                    this.logger?.Log(LogType.Error, "'{0}' is not a valid path.", x);
                }

                return null;
            }
        };

        public async Task Run(params string[] args)
        {
            int numberOfDoubleDashedArguments = args.Count(this.FilterDoubleDashedOption);
            if (numberOfDoubleDashedArguments != 1)
            {
                await this.helpProvider.GetHelp();
                return;
            }

            var journalId = args.Single(this.FilterDoubleDashedOption).Substring(2);

            IJournalMeta journalMeta = (await this.journalsMetaService.GetAllJournalsMeta())
                .FirstOrDefault(j => j.Permalink == journalId);

            if (journalMeta == null)
            {
                this.logger?.Log(LogType.Error, "Journal not found");
                return;
            }

            this.ProcessDirectories(args, journalMeta);
        }

        private void ProcessDirectories(string[] args, IJournalMeta journalMeta)
        {
            var directories = args.Where(this.FilterNonDashedOption)
                .Select(this.SelectDirectoryName)
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .ToArray();

            foreach (var directoryName in directories)
            {
                this.logger?.Log(directoryName);

                var direcoryProcessor = this.processorFactory.CreateDirectoryProcessor(directoryName, journalMeta);

                // Processing of each should be executed strictly sequential
                // due to the changes of the current location.
                direcoryProcessor.Process().Wait();
            }
        }
    }
}
