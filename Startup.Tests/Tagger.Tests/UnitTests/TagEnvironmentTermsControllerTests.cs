﻿namespace ProcessingTools.MainProgram.Tests.UnitTests
{
    using System;
    using System.Xml;

    using Controllers;
    using Moq;
    using NUnit.Framework;
    using ProcessingTools.Contracts;
    using ProcessingTools.Bio.Data.Miners.Contracts;

    [TestFixture]
    public class TagEnvironmentTermsControllerTests
    {
        private const string CallShouldThrowSystemAggregateExceptionMessage = "Call should throw System.AggregateException.";
        private const string CallShouldThrowSystemArgumentNullExceptionMessage = "Call should throw System.ArgumentNullException.";
        private const string InnerExceptionShouldBeArgumentNullExceptionMessage = "InnerException should be System.ArgumentNullException.";
        private const string ContentShouldBeUnchangedMessage = "Content should be unchaged.";

        private XmlDocument document;
        private XmlNamespaceManager namespaceManager;
        private ProgramSettings settings;
        private ILogger logger;

        private IEnvoTermsDataMiner miner;

        [SetUp]
        public void Init()
        {
            this.document = new XmlDocument();
            this.document.LoadXml("<root />");

            this.namespaceManager = new XmlNamespaceManager(this.document.NameTable);
            this.settings = new ProgramSettings();

            var loggerMock = new Mock<ILogger>();
            this.logger = loggerMock.Object;

            var minerMock = new Mock<IEnvoTermsDataMiner>();
            this.miner = minerMock.Object;
        }

        [Test]
        public void TagEnvironmentTermsController_WithDefaultCnstructor_ShouldReturnValidObject()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.IsNotNull(controller, "Controller should not be null.");
        }

        [Test]
        public void TagEnvironmentTermsController_WithNullService_ShouldThrowArgumentNullException()
        {


            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var controller = new TagEnvironmentTermsController(null);
                },
                CallShouldThrowSystemArgumentNullExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_WithNullService_ShouldThrowArgumentNullExceptionWithParamName()
        {
            try
            {
                var controller = new TagEnvironmentTermsController(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentNullException), e.GetType(), CallShouldThrowSystemArgumentNullExceptionMessage);

                Assert.AreEqual("miner", ((ArgumentNullException)e).ParamName, @"ParamName should be ""miner"".");
            }
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithValidParameters_ShouldWork()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            string initialContent = this.document.OuterXml;

            controller.Run(this.document.DocumentElement, this.namespaceManager, this.settings, this.logger).Wait();

            string finalContent = this.document.OuterXml;

            Assert.AreEqual(initialContent, finalContent, ContentShouldBeUnchangedMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullContextAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullContextAndNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullContextAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullContextAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullContextAndNullNamespaceManagerAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullContextAndNullNamespaceManagerAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullContextAndNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, this.namespaceManager, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(null, null, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullContextAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

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
        public void TagEnvironmentTermsController_RunWithNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, this.settings, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullNamespaceManagerAndNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullNamespaceManagerAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, this.settings, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullNamespaceManagerAndNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, null, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullNamespaceManagerAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

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
        public void TagEnvironmentTermsController_RunWithNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, this.namespaceManager, null, this.logger).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullProgramSettingsAndNullLoggerAndValidOtherParameters_ShouldThrowAggregateException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            Assert.Throws<AggregateException>(
                () => controller.Run(this.document.DocumentElement, this.namespaceManager, null, null).Wait(),
                CallShouldThrowSystemAggregateExceptionMessage);
        }

        [Test]
        public void TagEnvironmentTermsController_RunWithNullProgramSettingsAndValidOtherParameters_ShouldThrowAggregateExceptionWithInnerArgumentNullException()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

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
        public void TagEnvironmentTermsController_RunWithNullLoggerAndValidOtherParameters_ShouldWork()
        {
            var controller = new TagEnvironmentTermsController(this.miner);

            string initialContent = this.document.OuterXml;

            controller.Run(this.document.DocumentElement, this.namespaceManager, this.settings, null).Wait();

            string finalContent = this.document.OuterXml;

            Assert.AreEqual(initialContent, finalContent, ContentShouldBeUnchangedMessage);
        }
    }
}
