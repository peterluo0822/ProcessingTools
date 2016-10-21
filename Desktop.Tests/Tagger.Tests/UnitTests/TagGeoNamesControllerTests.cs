﻿namespace ProcessingTools.Tagger.Tests.UnitTests
{
    using System;
    using System.Xml;
    using Controllers;
    using Moq;
    using NUnit.Framework;
    using ProcessingTools.Contracts;
    using ProcessingTools.Geo.Data.Miners.Contracts;
    using ProcessingTools.Layout.Processors.Contracts.Taggers;

    [TestFixture]
    public class TagGeoNamesControllerTests
    {
        private const string CallShouldThrowSystemAggregateExceptionMessage = "Call should throw System.AggregateException.";
        private const string CallShouldThrowSystemArgumentNullExceptionMessage = "Call should throw System.ArgumentNullException.";
        private const string InnerExceptionShouldBeArgumentNullExceptionMessage = "InnerException should be System.ArgumentNullException.";
        private const string ContentShouldBeUnchangedMessage = "Content should be unchanged.";

        private XmlDocument document;
        private XmlNamespaceManager namespaceManager;
        private ProgramSettings settings;
        private ILogger logger;

        private IGeoNamesDataMiner miner;
        private IDocumentFactory documentFactory;
        private IStringTagger tagger;

        [SetUp]
        public void Init()
        {
            this.document = new XmlDocument();
            this.document.LoadXml("<root />");

            this.namespaceManager = new XmlNamespaceManager(this.document.NameTable);
            this.settings = new ProgramSettings();

            var loggerMock = new Mock<ILogger>();
            this.logger = loggerMock.Object;

            var minerMock = new Mock<IGeoNamesDataMiner>();
            this.miner = minerMock.Object;

            var documentFactoryMock = new Mock<IDocumentFactory>();
            this.documentFactory = documentFactoryMock.Object;

            var taggerMock = new Mock<IStringTagger>();
            this.tagger = taggerMock.Object;
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_WithDefaultCnstructor_ShouldReturnValidObject()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.IsNotNull(controller, "Controller should not be null.");
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_WithNullService_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var controller = new TagGeoNamesController(null, this.documentFactory, this.tagger);
                },
                CallShouldThrowSystemArgumentNullExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_WithNullService_ShouldThrowArgumentNullExceptionWithParamName()
        {
            try
            {
                var controller = new TagGeoNamesController(null, this.documentFactory, this.tagger);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentNullException), e.GetType(), CallShouldThrowSystemArgumentNullExceptionMessage);

                Assert.AreEqual("miner", ((ArgumentNullException)e).ParamName, @"ParamName should be ""miner"".");
            }
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithValidParameters_ShouldWork()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            string initialContent = this.document.OuterXml;

            controller.Run(this.document.DocumentElement, this.namespaceManager, this.settings, this.logger).Wait();

            string finalContent = this.document.OuterXml;

            Assert.AreEqual(initialContent, finalContent, ContentShouldBeUnchangedMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullContextAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullContextAndNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullContextAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullContextAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullContextAndNullNamespaceManagerAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullContextAndNullNamespaceManagerAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullContextAndNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullContextAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

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
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullNamespaceManagerAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullNamespaceManagerAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullNamespaceManagerAndNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

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
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, this.namespaceManager, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, this.namespaceManager, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

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
        [Timeout(500)]
        public void TagGeoNamesController_RunWithNullLoggerAndValidOtherParameters_ShouldWork()
        {
            var controller = new TagGeoNamesController(this.miner, this.documentFactory, this.tagger);

            string initialContent = this.document.OuterXml;

            controller.Run(this.document.DocumentElement, this.namespaceManager, this.settings, null).Wait();

            string finalContent = this.document.OuterXml;

            Assert.AreEqual(initialContent, finalContent, ContentShouldBeUnchangedMessage);
        }
    }
}
