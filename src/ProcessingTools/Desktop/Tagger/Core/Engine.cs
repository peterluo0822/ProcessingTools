﻿namespace ProcessingTools.Tagger.Core
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Contracts;
    using Core;
    using ProcessingTools.Constants.Configuration;
    using ProcessingTools.Contracts;
    using ProcessingTools.Enumerations;

    public class Engine : IEngine
    {
        private readonly IFileProcessor fileProcessor;
        private readonly ILogger logger;

        public Engine(IFileProcessor fileProcessor, ILogger logger)
        {
            if (fileProcessor == null)
            {
                throw new ArgumentNullException(nameof(fileProcessor));
            }

            this.fileProcessor = fileProcessor;
            this.logger = logger;
        }

        public void Run(string[] args)
        {
            int timeSpanInMunutesValue = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings[AppSettingsKeys.MaximalTimeInMinutesToWaitTheMainThread], out timeSpanInMunutesValue))
            {
                throw new SystemException("MaximalTimeInMinutesToWaitTheMainThread has invalid value.");
            }

            var ts = TimeSpan.FromMinutes(timeSpanInMunutesValue);

            var succeeded = this.RunAsync(args).Wait(ts);
            if (!succeeded)
            {
                this.logger.Log(LogType.Error, "The timeout interval elapsed.");
            }
        }

        private async Task RunAsync(string[] args)
        {
            try
            {
                var settingsBuilder = new ProgramSettingsBuilder(this.logger, args);
                var settings = settingsBuilder.Settings;

                await this.fileProcessor.Run(settings);
            }
            catch (Exception e)
            {
                this.logger.Log(e, string.Empty);
            }
        }
    }
}
