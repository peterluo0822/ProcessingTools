﻿namespace ProcessingTools.Web.Api.Controllers
{
    using ProcessingTools.Contracts;
    using ProcessingTools.Contracts.Services.Data.Bio.Taxonomy;
    using ProcessingTools.Web.Api.Abstractions;

    public class AphiaController : AbstractTaxaClassificationResolverController
    {
        protected AphiaController(IAphiaTaxaClassificationResolver resolver, ILogger logger)
            : base(resolver, logger)
        {
        }
    }
}
