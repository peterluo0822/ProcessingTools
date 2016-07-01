﻿namespace ProcessingTools.Web.Documents.Areas.Journals.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using ProcessingTools.Documents.Data;
    using ProcessingTools.Documents.Data.Contracts;
    using ProcessingTools.Documents.Data.Models;
    using ProcessingTools.Web.Common.Constants;

    using System.Linq;
    using System.Web;


    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using ProcessingTools.Common.Constants;
    using ProcessingTools.Common.Exceptions;
    using ProcessingTools.Documents.Services.Data.Contracts;
    using ProcessingTools.Documents.Services.Data.Models.Journals;
    using ProcessingTools.Extensions;
    using ProcessingTools.Web.Documents.Areas.Journals.ViewModels.Journals;
    using ProcessingTools.Web.Documents.Extensions;

    public class JournalsController : Controller
    {
        public const string InstanceName = "Journal";
        private readonly IJournalsDataService journalsDataService;
        private readonly IPublishersDataService publishersDataService;

        private readonly DocumentsDbContext db;

        public JournalsController(IJournalsDataService journalsDataService, IPublishersDataService publishersDataService)
        {
            if (journalsDataService == null)
            {
                throw new ArgumentNullException(nameof(journalsDataService));
            }

            if (publishersDataService == null)
            {
                throw new ArgumentNullException(nameof(publishersDataService));
            }

            this.journalsDataService = journalsDataService;
            this.publishersDataService = publishersDataService;
        }

        public static string ControllerName => ControllerConstants.JournalsControllerName;

        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        // GET: Journals/Journals
        public async Task<ActionResult> Index(int? p, int? n)
        {
            try
            {
                int pageNumber = p ?? PagingConstants.DefaultPageNumber;
                int itemsPerPage = n ?? PagingConstants.DefaultLargeNumberOfItemsPerPage;

                var viewModels = (await this.journalsDataService.All(pageNumber, itemsPerPage))
                    .Select(e => new JournalViewModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        AbbreviatedName = e.AbbreviatedName,
                        JournalId = e.JournalId,
                        PrintIssn = e.PrintIssn,
                        ElectronicIssn = e.ElectronicIssn,
                        DateCreated = e.DateCreated,
                        DateModified = e.DateModified,
                        Publisher = new PublisherViewModel
                        {
                            Id = e.Publisher.Id,
                            Name = e.Publisher.Name,
                            AbbreviatedName = e.Publisher.AbbreviatedName
                        }
                    })
                    .ToList();

                var numberOfDocuments = await this.journalsDataService.Count();

                this.ViewBag.PageNumber = pageNumber;
                this.ViewBag.NumberOfItemsPerPage = itemsPerPage;
                this.ViewBag.NumberOfPages = (numberOfDocuments % itemsPerPage) == 0 ? numberOfDocuments / itemsPerPage : (numberOfDocuments / itemsPerPage) + 1;
                this.ViewBag.ActionName = nameof(this.Index);

                this.Response.StatusCode = (int)HttpStatusCode.OK;
                return this.View(viewModels);
            }
            catch (InvalidPageNumberException e)
            {
                return this.InvalidPageNumberErrorView(InstanceName, e.Message, ContentConstants.DefaultBackToListActionLinkTitle, AreasConstants.JournalsAreaName);
            }
            catch (InvalidItemsPerPageException e)
            {
                return this.InvalidNumberOfItemsPerPageErrorView(InstanceName, e.Message, ContentConstants.DefaultBackToListActionLinkTitle, AreasConstants.JournalsAreaName);
            }
            catch (ArgumentException e)
            {
                return this.BadRequestErrorView(InstanceName, e.Message, ContentConstants.DefaultIndexActionLinkTitle, AreasConstants.JournalsAreaName);
            }
            catch (Exception e)
            {
                return this.DefaultErrorView(InstanceName, e.Message, ContentConstants.DefaultIndexActionLinkTitle, AreasConstants.JournalsAreaName);
            }
        }

        // GET: Journals/Journals/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return this.NullIdErrorView(InstanceName, string.Empty, ContentConstants.DefaultDetailsActionLinkTitle, AreasConstants.JournalsAreaName);
            }

            try
            {
                var serviceModel = await this.journalsDataService.GetDetails(id);
                var viewModel = await this.MapToDetailsViewModelWithoutCollections(serviceModel);

                // TODO: Is this needed here???
                viewModel.Articles = serviceModel.Articles
                    .Select(a => new ArticleViewModel
                    {
                        Id = a.Id,
                        Title = a.Title
                    })
                    .OrderBy(a => a.Title)
                    .ToList();

                return this.View(viewModel);
            }
            catch (EntityNotFoundException e)
            {
                return this.DefaultNotFoundView(InstanceName, e.Message, ContentConstants.DefaultDetailsActionLinkTitle, AreasConstants.JournalsAreaName);
            }
            catch (ArgumentException e)
            {
                return this.BadRequestErrorView(InstanceName, e.Message, ContentConstants.DefaultDetailsActionLinkTitle, AreasConstants.JournalsAreaName);
            }
            catch (Exception e)
            {
                return this.DefaultErrorView(InstanceName, e.Message, ContentConstants.DefaultDetailsActionLinkTitle, AreasConstants.JournalsAreaName);
            }
        }

        // GET: Journals/Journals/Create
        public async Task<ActionResult> Create()
        {
            var publishers = (await this.publishersDataService.All())
                .Select(p => new PublisherSimpleViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

            this.ViewBag.PublisherId = new SelectList(publishers, nameof(PublisherSimpleViewModel.Id), nameof(PublisherSimpleViewModel.Name));
            return this.View();
        }

        // POST: Journals/Journals/Create
        // To protect from over-posting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,AbbreviatedName,JournalId,PrintIssn,ElectronicIssn,PublisherId,CreatedByUserId,DateCreated,DateModified,ModifiedByUserId")] Journal journal)
        {
            if (this.ModelState.IsValid)
            {
                journal.Id = Guid.NewGuid();
                this.db.Journals.Add(journal);
                await this.db.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewBag.PublisherId = new SelectList(this.db.Publishers, "Id", "Name", journal.PublisherId);
            return this.View(journal);
        }

        // GET: Journals/Journals/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var journal = await Task.FromResult(this.db.Journals.Find(id));
            if (journal == null)
            {
                return this.HttpNotFound();
            }

            this.ViewBag.PublisherId = new SelectList(this.db.Publishers, "Id", "Name", journal.PublisherId);
            return this.View(journal);
        }

        // POST: Journals/Journals/Edit/5
        // To protect from over-posting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,AbbreviatedName,JournalId,PrintIssn,ElectronicIssn,PublisherId,CreatedByUserId,DateCreated,DateModified,ModifiedByUserId")] Journal journal)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Entry(journal).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewBag.PublisherId = new SelectList(this.db.Publishers, "Id", "Name", journal.PublisherId);
            return this.View(journal);
        }

        // GET: Journals/Journals/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var journal = await Task.FromResult(this.db.Journals.Find(id));
            if (journal == null)
            {
                return this.HttpNotFound();
            }

            return this.View(journal);
        }

        // POST: Journals/Journals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var journal = await Task.FromResult(this.db.Journals.Find(id));
            this.db.Journals.Remove(journal);
            await this.db.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        private async Task<JournalDetailsViewModel> MapToDetailsViewModelWithoutCollections(JournalServiceModel serviceModel)
        {
            string createdBy = (await this.UserManager.FindByIdAsync(serviceModel.CreatedByUser)).UserName;
            string modifiedBy = (await this.UserManager.FindByIdAsync(serviceModel.ModifiedByUser)).UserName;

            var model = new JournalDetailsViewModel
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                AbbreviatedName = serviceModel.AbbreviatedName,
                JournalId = serviceModel.JournalId,
                PrintIssn = serviceModel.PrintIssn,
                ElectronicIssn = serviceModel.ElectronicIssn,
                CreatedByUser = createdBy,
                ModifiedByUser = modifiedBy,
                DateCreated = serviceModel.DateCreated,
                DateModified = serviceModel.DateModified,
                Publisher = new PublisherViewModel
                {
                    Id = serviceModel.Publisher.Id,
                    Name = serviceModel.Publisher.Name,
                    AbbreviatedName = serviceModel.Publisher.AbbreviatedName
                }
            };

            return model;
        }
    }
}
