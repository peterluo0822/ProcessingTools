﻿namespace ProcessingTools.Harvesters.Harvesters.Abbreviations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Xml;
    using Abstractions;
    using Contracts.Factories;
    using Contracts.Harvesters.Abbreviations;
    using Contracts.Models.Abbreviations;
    using Models.Abbreviations;
    using ProcessingTools.Xml.Contracts.Serialization;
    using ProcessingTools.Xml.Contracts.Wrappers;

    public class AbbreviationsHarvester : AbstractGenericEnumerableXmlHarvester<IAbbreviationModel>, IAbbreviationsHarvester
    {
        private readonly IXmlTransformDeserializer serializer;
        private readonly IAbbreviationsTransformersFactory transformersFactory;

        public AbbreviationsHarvester(
            IXmlContextWrapper contextWrapper,
            IXmlTransformDeserializer serializer,
            IAbbreviationsTransformersFactory transformersFactory)
            : base(contextWrapper)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (transformersFactory == null)
            {
                throw new ArgumentNullException(nameof(transformersFactory));
            }

            this.serializer = serializer;
            this.transformersFactory = transformersFactory;
        }

        protected override async Task<IEnumerable<IAbbreviationModel>> Run(XmlDocument document)
        {
            var transformer = this.transformersFactory.GetAbbreviationsTransformer();
            var items = await this.serializer.Deserialize<AbbreviationsXmlModel>(transformer, document.DocumentElement);

            if (items?.Abbreviations == null)
            {
                return null;
            }

            var result = new HashSet<IAbbreviationModel>(items.Abbreviations);

            return result;
        }
    }
}
