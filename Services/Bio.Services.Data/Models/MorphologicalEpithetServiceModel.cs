﻿namespace ProcessingTools.Bio.Services.Data.Models
{
    using ProcessingTools.Services.Common.Models.Contracts;

    public class MorphologicalEpithetServiceModel : INamedDataServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
