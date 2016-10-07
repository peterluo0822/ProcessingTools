﻿namespace ProcessingTools.BaseLibrary.Floats.Models
{
    using Contracts;

    /// <summary>
    /// Movie.
    /// </summary>
    public class MovieFloatObject : IFloatObject
    {
        public string FloatObjectXPath => $".//fig[contains(string(label),'{this.FloatTypeNameInLabel}')]";

        public FloatsReferenceType FloatReferenceType => FloatsReferenceType.Figure;

        public string FloatTypeNameInLabel => "Movie";

        public string MatchCitationPattern => @"(?:Movies?)";

        public string InternalReferenceType => "movie";

        public string ResultantReferenceType => "fig";

        public string Description => "Movie";
    }
}
