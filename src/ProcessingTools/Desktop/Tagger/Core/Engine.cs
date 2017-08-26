﻿namespace ProcessingTools.Tagger.Core
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using ProcessingTools.Constants.Configuration;
    using ProcessingTools.Contracts;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Tagger.Contracts;

    public class Engine : IEngine
    {
        private readonly IFileProcessor fileProcessor;
        private readonly ILogger logger;

        public Engine(IFileProcessor fileProcessor, ILogger logger)
        {
            this.fileProcessor = fileProcessor ?? throw new ArgumentNullException(nameof(fileProcessor));
            this.logger = logger;
        }

        public void Run(string[] args)
        {
            if (!int.TryParse(ConfigurationManager.AppSettings[AppSettingsKeys.MaximalTimeInMinutesToWaitTheMainThread], out int timeSpanInMunutesValue))
            {
                throw new ConfigurationErrorsException("MaximalTimeInMinutesToWaitTheMainThread has invalid value.");
            }

            var ts = TimeSpan.FromMinutes(timeSpanInMunutesValue);

            var succeeded = this.RunAsync(args).Wait(ts);
            if (!succeeded)
            {
                this.logger.Log(LogType.Error, message: "The timeout interval elapsed.");
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
                this.logger.Log(e, message: string.Empty);
            }
        }
    }
}
