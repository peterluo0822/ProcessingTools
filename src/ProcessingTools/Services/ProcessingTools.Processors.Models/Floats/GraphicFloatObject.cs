﻿// <copyright file="GraphicFloatObject.cs" company="ProcessingTools">
// Copyright (c) 2017 ProcessingTools. All rights reserved.
// </copyright>

namespace ProcessingTools.Processors.Models.Floats
{
    using ProcessingTools.Constants.Schema;
    using ProcessingTools.Contracts.Processors.Models.Floats;
    using ProcessingTools.Enumerations.Nlm;

    /// <summary>
    /// Graphic.
    /// </summary>
    public class GraphicFloatObject : IFloatObject
    {
        /// <inheritdoc/>
        public string FloatObjectXPath => $".//fig[contains(string(label),'{this.FloatTypeNameInLabel}')]";

        /// <inheritdoc/>
        public ReferenceType FloatReferenceType => ReferenceType.Figure;

        /// <inheritdoc/>
        public string FloatTypeNameInLabel => "Graphic";

        /// <inheritdoc/>
        public string MatchCitationPattern => @"(?:Graphics?)";

        /// <inheritdoc/>
        public string InternalReferenceType => "graphic";

        /// <inheritdoc/>
        public string ResultantReferenceType => AttributeValues.RefTypeFigure;

        /// <inheritdoc/>
        public string Description => "Graphic";
    }
}