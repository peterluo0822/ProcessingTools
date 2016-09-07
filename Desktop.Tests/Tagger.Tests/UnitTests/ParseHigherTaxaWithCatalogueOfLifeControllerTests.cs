﻿namespace ProcessingTools.Tagger.Tests.UnitTests
{
    using System;
    using System.Xml;

    using Controllers;
    using Moq;
    using NUnit.Framework;
    using ProcessingTools.Bio.Taxonomy.Services.Data.Contracts;
    using ProcessingTools.Contracts;

    [TestFixture]
    public class ParseHigherTaxaWithCatalogueOfLifeControllerTests
    {
        private const string CallShouldThrowSystemAggregateExceptionMessage = "Call should throw System.AggregateException.";
        private const string CallShouldThrowSystemArgumentNullExceptionMessage = "Call should throw System.ArgumentNullException.";
        private const string InnerExceptionShouldBeArgumentNullExceptionMessage = "InnerException should be System.ArgumentNullException.";
        private const string ContentShouldBeUnchangedMessage = "Content should be unchanged.";

        private XmlDocument document;
        private XmlNamespaceManager namespaceManager;
        private ProgramSettings settings;
        private ILogger logger;

        private ICatalogueOfLifeTaxaRankResolverDataService service;

        [SetUp]
        public void Init()
        {
            this.document = new XmlDocument();
            this.document.LoadXml("<root />");

            this.namespaceManager = new XmlNamespaceManager(this.document.NameTable);
            this.settings = new ProgramSettings();

            var loggerMock = new Mock<ILogger>();
            this.logger = loggerMock.Object;

            var serviceMock = new Mock<ICatalogueOfLifeTaxaRankResolverDataService>();
            this.service = serviceMock.Object;
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_WithDefaultCnstructor_ShouldReturnValidObject()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.IsNotNull(controller, "Controller should not be null.");
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_WithNullService_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var controller = new ParseHigherTaxaWithCatalogueOfLifeController(null);
                },
                CallShouldThrowSystemArgumentNullExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_WithNullService_ShouldThrowArgumentNullExceptionWithParamName()
        {
            try
            {
                var controller = new ParseHigherTaxaWithCatalogueOfLifeController(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentNullException), e.GetType(), CallShouldThrowSystemArgumentNullExceptionMessage);

                Assert.AreEqual("service", ((ArgumentNullException)e).ParamName, @"ParamName should be ""service"".");
            }
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithValidParameters_ShouldWork()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            string initialContent = this.document.OuterXml;

            controller.Run(this.document.DocumentElement, this.namespaceManager, this.settings, this.logger).Wait();

            string finalContent = this.document.OuterXml;

            Assert.AreEqual(initialContent, finalContent, ContentShouldBeUnchangedMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullContextAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullContextAndNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullContextAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullContextAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullContextAndNullNamespaceManagerAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullContextAndNullNamespaceManagerAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullContextAndNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullContextAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            try
            {
                controller.Run(null, this.namespaceManager, this.settings, this.logger).Wait();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(AggregateException), e.GetType(), CallShouldThrowSystemAggregateExceptionMessage);

                Exception innerException = e.InnerException;
                Assert.AreEqual(typeof(ArgumentNullException), innerException.GetType(), InnerExceptionShouldBeArgumentNullExceptionMessage);

                Assert.AreEqual("context", ((ArgumentNullException)innerException).ParamName, @"ParamName should be ""context"".");
            }
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullNamespaceManagerAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullNamespaceManagerAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullNamespaceManagerAndNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            try
            {
                controller.Run(this.document.DocumentElement, null, this.settings, this.logger).Wait();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(AggregateException), e.GetType(), CallShouldThrowSystemAggregateExceptionMessage);

                Exception innerException = e.InnerException;
                Assert.AreEqual(typeof(ArgumentNullException), innerException.GetType(), InnerExceptionShouldBeArgumentNullExceptionMessage);

                Assert.AreEqual("namespaceManager", ((ArgumentNullException)innerException).ParamName, @"ParamName should be ""namespaceManager"".");
            }
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, this.namespaceManager, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, this.namespaceManager, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            try
            {
                controller.Run(this.document.DocumentElement, this.namespaceManager, null, this.logger).Wait();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(AggregateException), e.GetType(), CallShouldThrowSystemAggregateExceptionMessage);

                Exception innerException = e.InnerException;
                Assert.AreEqual(typeof(ArgumentNullException), innerException.GetType(), InnerExceptionShouldBeArgumentNullExceptionMessage);

                Assert.AreEqual("settings", ((ArgumentNullException)innerException).ParamName, @"ParamName should be ""settings"".");
            }
        }

        [Test]
        public void ParseHigherTaxaWithCatalogueOfLifeController_RunWithNullLoggerAndValidOtherParameters_ShouldWork()
        {
            var controller = new ParseHigherTaxaWithCatalogueOfLifeController(this.service);

            string initialContent = this.document.OuterXml;

            controller.Run(this.document.DocumentElement, this.namespaceManager, this.settings, null).Wait();

            string finalContent = this.document.OuterXml;

            Assert.AreEqual(initialContent, finalContent, ContentShouldBeUnchangedMessage);
        }
    }
}