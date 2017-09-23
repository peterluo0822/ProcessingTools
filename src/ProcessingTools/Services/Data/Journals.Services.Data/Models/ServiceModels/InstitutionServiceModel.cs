﻿namespace ProcessingTools.Journals.Services.Data.Models.ServiceModels
{
    using Contracts.Models;
    using ProcessingTools.Models.Contracts;

    internal class InstitutionServiceModel : IInstitution, IServiceModel
    {
        public string AbbreviatedName { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
