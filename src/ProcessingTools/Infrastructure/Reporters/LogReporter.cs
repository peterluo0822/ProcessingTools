﻿namespace ProcessingTools.Reporters
{
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using ProcessingTools.Contracts;

    public class LogReporter : ILogReporter
    {
        private readonly ILogger logger;
        private readonly StringBuilder stringBuilder;

        public LogReporter(ILogger logger)
        {
            this.logger = logger;
            this.stringBuilder = new StringBuilder();
        }

        public void AppendContent(string content)
        {
            this.stringBuilder.AppendLine(content);
        }

        public async Task MakeReport() => await Task.Run(() => this.logger?.Log(message: this.stringBuilder.ToString()));
    }
}
