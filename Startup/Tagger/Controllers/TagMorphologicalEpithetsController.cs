﻿namespace ProcessingTools.Tagger.Controllers
{
    using System;
    using System.Xml;

    using Contracts;
    using Factories;

    using ProcessingTools.Attributes;
    using ProcessingTools.Bio.Data.Miners.Contracts;
    using ProcessingTools.Data.Miners.Common.Contracts;

    [Description("Tag morphological epithets.")]
    public class TagMorphologicalEpithetsController : StringTaggerControllerFactory, ITagMorphologicalEpithetsController
    {
        private readonly IMorphologicalEpithetsDataMiner miner;
        private readonly XmlElement tagModel;

        public TagMorphologicalEpithetsController(IMorphologicalEpithetsDataMiner miner)
        {
            if (miner == null)
            {
                throw new ArgumentNullException(nameof(miner));
            }

            this.miner = miner;

            XmlDocument document = new XmlDocument();
            this.tagModel = document.CreateElement("named-content");
            this.tagModel.SetAttribute("content-type", "morphological epithet");
        }

        protected override IStringDataMiner Miner => this.miner;

        protected override XmlElement TagModel => this.tagModel;
    }
}
