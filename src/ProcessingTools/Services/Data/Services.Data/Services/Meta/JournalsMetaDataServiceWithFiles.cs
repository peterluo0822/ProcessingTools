﻿namespace ProcessingTools.Services.Data.Services.Meta
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Meta;
    using ProcessingTools.Contracts.Models.Documents;

    public class JournalsMetaDataServiceWithFiles : IJournalsMetaDataService
    {
        private readonly IJournalMetaDataService journalMetaDataService;
        private string journalMetaFilesDirectory;

        public JournalsMetaDataServiceWithFiles(string journalMetaFilesDirectory, IJournalMetaDataService journalMetaDataService)
        {
            if (journalMetaDataService == null)
            {
                throw new ArgumentNullException(nameof(journalMetaDataService));
            }

            this.JournalMetaFilesDirectory = journalMetaFilesDirectory;
            this.journalMetaDataService = journalMetaDataService;
        }

        private string JournalMetaFilesDirectory
        {
            get
            {
                return this.journalMetaFilesDirectory;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(this.JournalMetaFilesDirectory));
                }

                if (!Directory.Exists(value))
                {
                    throw new DirectoryNotFoundException(value);
                }

                this.journalMetaFilesDirectory = value;
            }
        }

        public async Task<IEnumerable<IJournalMeta>> GetAllJournalsMeta()
        {
            var journalMetaFiles = Directory.GetFiles(this.JournalMetaFilesDirectory).ToArray();

            var result = new HashSet<IJournalMeta>();
            foreach (var fileName in journalMetaFiles)
            {
                var journalMeta = await this.journalMetaDataService.GetJournalMeta(fileName);
                result.Add(journalMeta);
            }

            return result;
        }
    }
}
