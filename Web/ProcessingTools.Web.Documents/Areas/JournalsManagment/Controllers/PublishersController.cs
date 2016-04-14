﻿namespace ProcessingTools.Web.Documents.Areas.JournalsManagment.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using ProcessingTools.Documents.Data;
    using ProcessingTools.Documents.Data.Models;

    [Authorize]
    public class PublishersController : Controller
    {
        private DocumentsDbContext db = new DocumentsDbContext();

        // GET: Publishers
        public ActionResult Index()
        {
            return this.View(this.db.Publishers.ToList());
        }

        // GET: Publishers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publisher publisher = this.db.Publishers.Find(id);
            if (publisher == null)
            {
                return this.HttpNotFound();
            }

            return this.View(publisher);
        }

        // GET: Publishers/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,AbbreviatedName")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                publisher.Id = Guid.NewGuid();
                publisher.CreatedByUserId = this.GetUserId();
                publisher.ModifiedByUserId = publisher.CreatedByUserId;
                publisher.DateCreated = DateTime.UtcNow;
                publisher.DateModified = publisher.DateCreated;

                this.db.Publishers.Add(publisher);
                this.db.SaveChanges();

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(publisher);
        }

        // GET: Publishers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publisher publisher = this.db.Publishers.Find(id);
            if (publisher == null)
            {
                return this.HttpNotFound();
            }

            return this.View(publisher);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,AbbreviatedName")] Publisher publisher)
        {
            Publisher entity = this.db.Publishers.Find(publisher.Id);
            if (ModelState.IsValid)
            {
                entity.Name = publisher.Name;
                entity.AbbreviatedName = publisher.AbbreviatedName;
                entity.ModifiedByUserId = this.GetUserId();
                entity.DateModified = DateTime.UtcNow;

                this.db.Entry(entity).State = EntityState.Modified;
                this.db.SaveChanges();

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(publisher);
        }

        // GET: Publishers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publisher publisher = this.db.Publishers.Find(id);
            if (publisher == null)
            {
                return this.HttpNotFound();
            }

            return this.View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Publisher publisher = this.db.Publishers.Find(id);
            this.db.Publishers.Remove(publisher);
            this.db.SaveChanges();

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

        private string GetUserId()
        {
            int lengthOfGuid = Guid.NewGuid().ToString().Length;
            string name = User.Identity.Name;

            return name.Substring(0, Math.Min(name.Length, lengthOfGuid));
        }
    }
}
