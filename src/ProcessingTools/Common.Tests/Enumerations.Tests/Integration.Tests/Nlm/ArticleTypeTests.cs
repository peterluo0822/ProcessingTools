﻿namespace ProcessingTools.Enumerations.Tests.Integration.Tests.Nlm
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ProcessingTools.Common.Extensions;
    using ProcessingTools.Enumerations.Nlm;

    [TestClass]
    public class ArticleTypeTests
    {
        [TestMethod]
        public void ArticleType_AbstractMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("abstract", ArticleType.Abstract.GetName());
        }

        [TestMethod]
        public void ArticleType_AddendumMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("addendum", ArticleType.Addendum.GetName());
        }

        [TestMethod]
        public void ArticleType_AnnouncementMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("announcement", ArticleType.Announcement.GetName());
        }

        [TestMethod]
        public void ArticleType_ArticleCommentaryMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("article-commentary", ArticleType.ArticleCommentary.GetName());
        }

        [TestMethod]
        public void ArticleType_BookReviewMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("book-review", ArticleType.BookReview.GetName());
        }

        [TestMethod]
        public void ArticleType_BooksReceivedMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("books-received", ArticleType.BooksReceived.GetName());
        }

        [TestMethod]
        public void ArticleType_BriefReportMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("brief-report", ArticleType.BriefReport.GetName());
        }

        [TestMethod]
        public void ArticleType_CalendarMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("calendar", ArticleType.Calendar.GetName());
        }

        [TestMethod]
        public void ArticleType_CaseReportMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("case-report", ArticleType.CaseReport.GetName());
        }

        [TestMethod]
        public void ArticleType_CollectionMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("collection", ArticleType.Collection.GetName());
        }

        [TestMethod]
        public void ArticleType_CorrectionMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("correction", ArticleType.Correction.GetName());
        }

        [TestMethod]
        public void ArticleType_DiscussionMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("discussion", ArticleType.Discussion.GetName());
        }

        [TestMethod]
        public void ArticleType_DissertationMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("dissertation", ArticleType.Dissertation.GetName());
        }

        [TestMethod]
        public void ArticleType_EditorialMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("editorial", ArticleType.Editorial.GetName());
        }

        [TestMethod]
        public void ArticleType_InBriefMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("in-brief", ArticleType.InBrief.GetName());
        }

        [TestMethod]
        public void ArticleType_IntroductionMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("introduction", ArticleType.Introduction.GetName());
        }

        [TestMethod]
        public void ArticleType_LetterMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("letter", ArticleType.Letter.GetName());
        }

        [TestMethod]
        public void ArticleType_MeetingReportMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("meeting-report", ArticleType.MeetingReport.GetName());
        }

        [TestMethod]
        public void ArticleType_NewsMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("news", ArticleType.News.GetName());
        }

        [TestMethod]
        public void ArticleType_ObituaryMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("obituary", ArticleType.Obituary.GetName());
        }

        [TestMethod]
        public void ArticleType_OrationMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("oration", ArticleType.Oration.GetName());
        }

        [TestMethod]
        public void ArticleType_PartialRetractionMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("partial-retraction", ArticleType.PartialRetraction.GetName());
        }

        [TestMethod]
        public void ArticleType_ProductReviewMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("product-review", ArticleType.ProductReview.GetName());
        }

        [TestMethod]
        public void ArticleType_RapidCommunicationMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("rapid-communication", ArticleType.RapidCommunication.GetName());
        }

        [TestMethod]
        public void ArticleType_ReplyMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("reply", ArticleType.Reply.GetName());
        }

        [TestMethod]
        public void ArticleType_ReprintMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("reprint", ArticleType.Reprint.GetName());
        }

        [TestMethod]
        public void ArticleType_ResearchArticleMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("research-article", ArticleType.ResearchArticle.GetName());
        }

        [TestMethod]
        public void ArticleType_RetractionMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("retraction", ArticleType.Retraction.GetName());
        }

        [TestMethod]
        public void ArticleType_ReviewArticleMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("review-article", ArticleType.ReviewArticle.GetName());
        }

        [TestMethod]
        public void ArticleType_TranslationMember_ShouldHaveCorrectStringValue()
        {
            Assert.AreEqual("translation", ArticleType.Translation.GetName());
        }
    }
}
