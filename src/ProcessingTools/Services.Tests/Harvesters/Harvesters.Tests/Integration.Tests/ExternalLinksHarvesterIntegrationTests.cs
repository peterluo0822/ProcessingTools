﻿namespace ProcessingTools.Harvesters.Tests.Integration.Tests
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ProcessingTools.Constants.Configuration;
    using ProcessingTools.Harvesters.Contracts.Factories;
    using ProcessingTools.Harvesters.Harvesters.ExternalLinks;
    using ProcessingTools.Serialization.Serializers;
    using ProcessingTools.Xml.Cache;
    using ProcessingTools.Xml.Serialization;
    using ProcessingTools.Xml.Transformers;
    using ProcessingTools.Xml.Wrappers;

    [TestClass]
    public class ExternalLinksHarvesterIntegrationTests
    {
        [TestMethod]
        [Timeout(5000)]
        public void ExternalLinksHarvester_WithSampleXmlFile_ShouldExtractCorrectlyExternalLinks()
        {
            // Arrange
            const int ExpectedNumberOfExternalLinks = 10;
            const int ExpectedNumberOfExternalLinksOfTypeDoi = 9;

            var xmlFileName = $"{ConfigurationManager.AppSettings["SampleFiles"]}/article-with-external-links.xml";
            var document = new XmlDocument
            {
                PreserveWhitespace = true
            };

            document.Load(xmlFileName);

            var contextWrapper = new XmlContextWrapper();

            var deserializer = new XmlDeserializer();
            var serializer = new XmlTransformDeserializer(deserializer);

            var xslCache = new XslTransformCache();
            var transformer = new XslTransformer(
                ConfigurationManager.AppSettings[AppSettingsKeys.ExternalLinksXslFileName],
                xslCache);
            var transformersFactoryMock = new Mock<IExternalLinksTransformersFactory>();
            transformersFactoryMock
                .Setup(f => f.GetExternalLinksTransformer())
                .Returns(transformer);

            var harvester = new ExternalLinksHarvester(contextWrapper, serializer, transformersFactoryMock.Object);

            // Act
            var externalLinks = harvester.Harvest(document.DocumentElement).Result?.ToList();

            // Assert
            Assert.IsNotNull(externalLinks);
            externalLinks.ForEach(i => Console.WriteLine("{0} | {1} | {2}", i.BaseAddress, i.Uri, i.Value));

            Assert.AreEqual(ExpectedNumberOfExternalLinks, externalLinks.Count);

            var doiExternalLinks = externalLinks.Where(i => i.BaseAddress.Contains("doi.org")).ToList();
            Assert.AreEqual(ExpectedNumberOfExternalLinksOfTypeDoi, doiExternalLinks.Count);
        }
    }
}
