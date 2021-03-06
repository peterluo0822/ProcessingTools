﻿namespace ProcessingTools.Enumerations.Nlm
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Indicates what kind of article this is, for example,
    /// a research article, a commentary, a book or product review,
    /// a case report, a calendar, etc.
    /// </summary>
    public enum ArticleType
    {
        /// <summary>
        /// The article itself is an abstract (of a paper or presentation) that usually has been presented or published separately.
        /// </summary>
        [Display(Name = "abstract")]
        Abstract,

        /// <summary>
        /// A published work that adds additional information or clarification to another work (The somewhat similar value “correction” corrects an error in previously published material.)
        /// </summary>
        [Display(Name = "addendum")]
        Addendum,

        /// <summary>
        /// Material announced in the publication (may or may not be directly related to the publication)
        /// </summary>
        [Display(Name = "announcement")]
        Announcement,

        /// <summary>
        /// An work whose subject or focus is another article or articles; this article comments on the other article(s). (This value would be used when the editors of a publication invite an author with an opposing opinion to comment on a controversial article, and then publish the two together. The somewhat similar value “editorial” is reserved for commentary written by an editor or other publication staff.)
        /// </summary>
        [Display(Name = "article-commentary")]
        ArticleCommentary,

        /// <summary>
        /// Review or analysis of one or more printed or online books (The similar value “product-review” is used for product analyses.)
        /// </summary>
        [Display(Name = "book-review")]
        BookReview,

        /// <summary>
        /// Notification that items, e.g., books or other works, have been received by a publication for review or other consideration
        /// </summary>
        [Display(Name = "books-received")]
        BooksReceived,

        /// <summary>
        /// A short and/or rapid announcement of research results
        /// </summary>
        [Display(Name = "brief-report")]
        BriefReport,

        /// <summary>
        /// A list of events
        /// </summary>
        [Display(Name = "calendar")]
        Calendar,

        /// <summary>
        /// Case study, case report, or other description of a case
        /// </summary>
        [Display(Name = "case-report")]
        CaseReport,

        /// <summary>
        /// Wrapper article for a series of sub-articles or responses; this value’s usage is restricted to articles whose intellectual content appears primarily in <sub-article> or <response>.
        /// </summary>
        [Display(Name = "collection")]
        Collection,

        /// <summary>
        /// A modification or correction of previously published material; this is sometimes called “errata” (The somewhat similar value “addendum” merely adds to previously published material.)
        /// </summary>
        [Display(Name = "correction")]
        Correction,

        /// <summary>
        /// Invited discussion related to a specific article or issue
        /// </summary>
        [Display(Name = "discussion")]
        Discussion,

        /// <summary>
        /// Thesis or dissertation written as part of the completion of a degree of study
        /// </summary>
        [Display(Name = "dissertation")]
        Dissertation,

        /// <summary>
        /// Opinion piece, policy statement, or general commentary, typically written by staff of the publication (The similar value “article-commentary” is reserved for a commentary on a specific article or articles, which is written by an author with a contrasting position, not an editor or other publication staff.)
        /// </summary>
        [Display(Name = "editorial")]
        Editorial,

        /// <summary>
        /// Summary or teaser of items in the current issue
        /// </summary>
        [Display(Name = "in-brief")]
        InBrief,

        /// <summary>
        /// An introduction to a publication, or to a series of articles within a publication, etc., typically for a special section or issue
        /// </summary>
        [Display(Name = "introduction")]
        Introduction,

        /// <summary>
        /// Letter to a publication, typically commenting upon a published work
        /// </summary>
        [Display(Name = "letter")]
        Letter,

        /// <summary>
        /// Report of a conference, symposium, or meeting
        /// </summary>
        [Display(Name = "meeting-report")]
        MeetingReport,

        /// <summary>
        /// News item, normally current but atypically historical
        /// </summary>
        [Display(Name = "news")]
        News,

        /// <summary>
        /// Announcement of a death, or the appreciation for a colleague who has recently died
        /// </summary>
        [Display(Name = "obituary")]
        Obituary,

        /// <summary>
        /// Reprint of a speech or oral presentation
        /// </summary>
        [Display(Name = "oration")]
        Oration,

        /// <summary>
        /// Retraction or disavowal of part(s) of previously published material
        /// </summary>
        [Display(Name = "partial-retraction")]
        PartialRetraction,

        /// <summary>
        /// Description, analysis, or review of a product or service, for example, a software package (The similar value “book-review” is used for analyses of books.)
        /// </summary>
        [Display(Name = "product-review")]
        ProductReview,

        /// <summary>
        /// Fast-breaking research update or other news item
        /// </summary>
        [Display(Name = "rapid-communication")]
        RapidCommunication,

        /// <summary>
        /// Reply to a letter or commentary, typically by the original author commenting upon the comments
        /// </summary>
        [Display(Name = "reply")]
        Reply,

        /// <summary>
        /// Reprint of a previously published article
        /// </summary>
        [Display(Name = "reprint")]
        Reprint,

        /// <summary>
        /// Article reporting on primary research (The related value “review-article” describes a literature review, research summary, or state-of-the-art article.)
        /// </summary>
        [Display(Name = "research-article")]
        ResearchArticle,

        /// <summary>
        /// Retraction or disavowal of previously published material
        /// </summary>
        [Display(Name = "retraction")]
        Retraction,

        /// <summary>
        /// Review or state-of-the-art summary article (The related value “research-article” describes original research.)
        /// </summary>
        [Display(Name = "review-article")]
        ReviewArticle,

        /// <summary>
        /// Translation of an article originally produced in a different language
        /// </summary>
        [Display(Name = "translation")]
        Translation
    }
}
