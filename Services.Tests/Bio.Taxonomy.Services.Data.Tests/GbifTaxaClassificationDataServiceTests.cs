﻿namespace ProcessingTools.Bio.Taxonomy.Services.Data.Tests
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProcessingTools.Bio.Taxonomy.ServiceClient.Gbif;

    [TestClass]
    public class GbifTaxaClassificationDataServiceTests
    {
        [TestMethod]
        public void GbifTaxaClassificationDataService_DefaultConstructor_ShouldWork()
        {
            var requester = new GbifDataRequester();
            var service = new GbifTaxaClassificationDataService(requester);
            Assert.IsNotNull(service, "Service should not be null");
        }

        [TestMethod]
        [Timeout(5000)]
        [Ignore]
        public void GbifTaxaClassificationDataService_Resolve_ShouldWork()
        {
            const string CanonicalName = "Coleoptera";
            const string Rank = "order";

            var requester = new GbifDataRequester();
            var service = new GbifTaxaClassificationDataService(requester);
            var response = service.Resolve(CanonicalName).Result;

            var defaultClassification = response.FirstOrDefault();

            Assert.AreEqual(CanonicalName, defaultClassification.CanonicalName, "CanonicalName should match.");
            Assert.AreEqual(Rank, defaultClassification.Rank, "Rank should match.");
        }
    }
}