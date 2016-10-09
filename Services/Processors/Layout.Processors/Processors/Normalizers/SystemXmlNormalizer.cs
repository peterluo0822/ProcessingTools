﻿namespace ProcessingTools.Layout.Processors.Normalizers
{
    using Abstracts.Normalizers;
    using Contracts.Normalizers;
    using Contracts.Transformers;

    public class SystemXmlNormalizer : AbstractXmlNormalizer, ISystemXmlNormalizer
    {
        public SystemXmlNormalizer(IFormatToSystemTransformer transformer)
            : base(transformer)
        {
        }
    }
}