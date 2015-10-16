﻿namespace ProcessingTools.BaseLibrary.Taxonomy
{
    using System.Collections.Generic;
    using Configurator;
    using Globals;
    using Globals.Json.Gbif;

    public class GbifTreatmentMetaParser : TreatmentMetaParser
    {
        private ILogger logger;

        public GbifTreatmentMetaParser(string xml, ILogger logger)
            : base(xml, logger)
        {
            this.logger = logger;
        }

        public GbifTreatmentMetaParser(Config config, string xml, ILogger logger)
            : base(config, xml, logger)
        {
            this.logger = logger;
        }

        public GbifTreatmentMetaParser(IBase baseObject, ILogger logger)
            : base(baseObject, logger)
        {
            this.logger = logger;
        }

        public override void Parse()
        {
            try
            {
                IEnumerable<string> genusList = this.XmlDocument.GetStringListOfUniqueXmlNodes(SelectTreatmentGeneraXPathString, this.NamespaceManager);

                foreach (string genus in genusList)
                {
                    this.Delay();

                    this.logger?.Log("\n{0}\n", genus);

                    GbifResult gbifResult = Net.SearchGbif(genus);
                    if ((gbifResult != null) && (gbifResult.canonicalName != null || gbifResult.scientificName != null))
                    {
                        this.logger?.Log(
                            "{0} .... {1} .... {2}",
                            genus,
                            gbifResult.scientificName,
                            gbifResult.canonicalName);

                        if (!gbifResult.canonicalName.Equals(genus) && !gbifResult.scientificName.Contains(genus))
                        {
                            this.logger?.Log("No match.");
                        }
                        else
                        {
                            this.logger?.Log(
                                "Kingdom: {0}\nOrder: {1}\nFamily: {2}\n",
                                gbifResult.kingdom,
                                gbifResult.order,
                                gbifResult.family);

                            List<string> responseKingdom = new List<string>();
                            responseKingdom.Add(gbifResult.kingdom);

                            List<string> responseOrder = new List<string>();
                            responseOrder.Add(gbifResult.order);

                            List<string> responseFamily = new List<string>();
                            responseFamily.Add(gbifResult.family);

                            if (gbifResult.alternatives != null)
                            {
                                foreach (var alternative in gbifResult.alternatives)
                                {
                                    if (alternative.canonicalName.Equals(genus) || alternative.scientificName.Contains(genus))
                                    {
                                        responseKingdom.Add(alternative.kingdom);
                                        responseOrder.Add(alternative.order);
                                        responseFamily.Add(alternative.family);
                                    }
                                }
                            }

                            this.ReplaceTreatmentMetaClassificationItem(responseKingdom, genus, "kingdom");
                            this.ReplaceTreatmentMetaClassificationItem(responseOrder, genus, "order");
                            this.ReplaceTreatmentMetaClassificationItem(responseFamily, genus, "family");
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}