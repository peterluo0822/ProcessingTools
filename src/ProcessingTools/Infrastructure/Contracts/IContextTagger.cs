﻿namespace ProcessingTools.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Tag content in context object.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface IContextTagger<TContext, TResult>
    {
        /// <summary>
        /// Executes tagging operation over the context.
        /// </summary>
        /// <param name="context">Context object to be processed.</param>
        /// <returns>Task of result.</returns>
        Task<TResult> Tag(TContext context);
    }
}
