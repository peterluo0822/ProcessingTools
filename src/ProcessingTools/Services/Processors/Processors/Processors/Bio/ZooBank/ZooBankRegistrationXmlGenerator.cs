﻿namespace ProcessingTools.Processors.Processors.Bio.ZooBank
{
    using System;
    using System.Threading.Tasks;
    using ProcessingTools.Contracts;
    using ProcessingTools.Processors.Contracts.Factories;
    using ProcessingTools.Processors.Contracts.Processors.Bio.ZooBank;

    public class ZooBankRegistrationXmlGenerator : IZooBankRegistrationXmlGenerator
    {
        private readonly IRegistrationTransformersFactory transformerFactory;

        public ZooBankRegistrationXmlGenerator(IRegistrationTransformersFactory transformerFactory)
        {
            this.transformerFactory = transformerFactory ?? throw new ArgumentNullException(nameof(transformerFactory));
        }

        public async Task<object> Generate(IDocument context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var content = await this.transformerFactory
                .GetZooBankRegistrationTransformer()
                .Transform(context.Xml);

            context.Xml = content;

            return true;
        }
    }
}
