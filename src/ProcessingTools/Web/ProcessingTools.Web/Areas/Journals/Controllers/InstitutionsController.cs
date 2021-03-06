﻿namespace ProcessingTools.Web.Areas.Journals.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using ProcessingTools.Journals.Data.Entity;
    using ProcessingTools.Journals.Data.Entity.Models;

    [Authorize]
    public class InstitutionsController : Controller
    {
        private JournalsDbContext db = new JournalsDbContext("JournalsDatabaseConnection");

        // GET: Journals/Institutions
        public ActionResult Index()
        {
            return this.View(this.db.Institutions.ToList());
        }

        // GET: Journals/Institutions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Institution institution = this.db.Institutions.Find(id);
            if (institution == null)
            {
                return this.HttpNotFound();
            }

            return this.View(institution);
        }

        // GET: Journals/Institutions/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Journals/Institutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AbbreviatedName,Name,DateCreated,DateModified,CreatedByUser,ModifiedByUser")] Institution institution)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Institutions.Add(institution);
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(institution);
        }

        // GET: Journals/Institutions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Institution institution = this.db.Institutions.Find(id);
            if (institution == null)
            {
                return this.HttpNotFound();
            }

            return this.View(institution);
        }

        // POST: Journals/Institutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AbbreviatedName,Name,DateCreated,DateModified,CreatedByUser,ModifiedByUser")] Institution institution)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Entry(institution).State = EntityState.Modified;
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(institution);
        }

        // GET: Journals/Institutions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Institution institution = this.db.Institutions.Find(id);
            if (institution == null)
            {
                return this.HttpNotFound();
            }

            return this.View(institution);
        }

        // POST: Journals/Institutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Institution institution = this.db.Institutions.Find(id);
            this.db.Institutions.Remove(institution);
            this.db.SaveChanges();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
