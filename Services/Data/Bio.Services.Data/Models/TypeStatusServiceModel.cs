﻿namespace ProcessingTools.Bio.Services.Data.Models
{
    using ProcessingTools.Contracts;

    public class TypeStatusServiceModel : INameableIntegerIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
