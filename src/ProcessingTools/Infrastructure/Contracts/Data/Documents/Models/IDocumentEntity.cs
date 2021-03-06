﻿namespace ProcessingTools.Contracts.Data.Documents.Models
{
    using System;
    using ProcessingTools.Contracts.Models;

    // TODO: separation with IFileEntity
    public interface IDocumentEntity : IGuidIdentifiable, ICommentable, IModelWithUserInformation
    {
        Guid ArticleId { get; }

        // TODO: add
        // Guid FileId { get; }

        // TODO: add
        // string Description { get; }

        // TODO: remove
        string FilePath { get; }

        // TODO: remove
        long ContentLength { get; }

        // TODO: remove
        string ContentType { get; }

        // TODO: remove
        string FileExtension { get; }

        // TODO: remove
        string FileName { get; }

        // TODO: remove
        long OriginalContentLength { get; }

        // TODO: remove
        string OriginalContentType { get; }

        // TODO: remove
        string OriginalFileExtension { get; }

        // TODO: remove
        string OriginalFileName { get; }
    }
}
