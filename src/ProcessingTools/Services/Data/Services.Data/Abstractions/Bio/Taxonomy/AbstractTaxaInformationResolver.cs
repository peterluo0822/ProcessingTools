﻿namespace ProcessingTools.Services.Data.Abstractions.Bio.Taxonomy
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts.Services.Data.Bio.Taxonomy;

    public abstract class AbstractTaxaInformationResolver<T> : ITaxaInformationResolver<T>
    {
        public async Task<T[]> ResolveAsync(params string[] scientificNames)
        {
            var queue = new ConcurrentQueue<T>();
            var exceptions = new ConcurrentQueue<Exception>();
            var tasks = scientificNames
                .Select(scientificName => this.ResolveScientificName(scientificName, queue, exceptions))
                .ToArray();

            await Task.WhenAll(tasks);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions.ToList());
            }

            return queue.ToArray();
        }

        protected abstract Task<T[]> ResolveScientificNameAsync(string scientificName);

        private async Task ResolveScientificName(string scientificName, ConcurrentQueue<T> queue, ConcurrentQueue<Exception> exceptions)
        {
            try
            {
                var data = await this.ResolveScientificNameAsync(scientificName).ConfigureAwait(false);
                foreach (var item in data)
                {
                    queue.Enqueue(item);
                }
            }
            catch (Exception e)
            {
                exceptions.Enqueue(e);
            }
        }
    }
}
