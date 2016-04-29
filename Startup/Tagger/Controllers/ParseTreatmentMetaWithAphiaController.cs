﻿namespace ProcessingTools.Tagger.Controllers
{
    using System;

    using Contracts;
    using Factories;

    using ProcessingTools.Attributes;
    using ProcessingTools.Bio.Taxonomy.Contracts;
    using ProcessingTools.Bio.Taxonomy.Services.Data.Contracts;

    [Description("Parse treatment meta with Aphia.")]
    public class ParseTreatmentMetaWithAphiaController : ParseTreatmentMetaControllerFactory, IParseTreatmentMetaWithAphiaController
    {
        private readonly IAphiaTaxaClassificationDataService service;

        public ParseTreatmentMetaWithAphiaController(IAphiaTaxaClassificationDataService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this.service = service;
        }

        protected override ITaxaInformationResolverDataService<ITaxonClassification> Service => this.service;
    }
}