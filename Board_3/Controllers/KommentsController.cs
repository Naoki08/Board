using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Board_3.Models;
using PagedList;

namespace Board_3.Controllers
{
    public class KommentsController : Controller
    {
        private BDBContext db = new BDBContext();


        // GET: Komments

        [Route("~/index")]
        [Route("~/index/page{page}")]
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            if(pageNumber < 1)
            {
                pageNumber = 1;
            }
            int pageSize = 3;

            if ((Account)Session["Account"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var komments = db.Komments.Include(k => k.Account).OrderByDescending(k => k.Created);

            return View(komments.ToPagedList(pageNumber, pageSize));
        }

        // GET: Komments/Details/5
        public ActionResult Details(int? id)
        {
            Komment nowAccount = db.Komments.Find(id);
            if (nowAccount == null || ((Account)Session["Account"]).AccountId != nowAccount.AccountId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komment komment = db.Komments.Find(id);
            if (komment == null)
            {
                return HttpNotFound();
            }
            return View(komment);
        }

        // GET: Komments/Create
        public ActionResult Create()
        {
            
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "Name");
            return View();
        }

        // POST: Komments/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Body,AccountId")] Komment komment)
        {

            komment.Created = DateTime.UtcNow.AddHours(8);
            komment.AccountId = ((Account)Session["Account"]).AccountId;
            if (ModelState.IsValid)
            {
                db.Komments.Add(komment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "Name", komment.AccountId);
            return View(komment);
        }

        // GET: Komments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Komment nowAccount = db.Komments.Find(id);
            if (nowAccount == null || ((Account)Session["Account"]).AccountId != nowAccount.AccountId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komment komment = db.Komments.Find(id);
            if (komment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "Name", komment.AccountId);
            return View(komment);
        }

        // POST: Komments/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KommentId,Body,AccountId")] Komment komment)
        {
            komment.Created = (DateTime.UtcNow.AddHours(8));
            komment.AccountId = ((Account)Session["Account"]).AccountId;

            if (ModelState.IsValid)
            {
                db.Entry(komment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "Name", komment.AccountId);
            return View(komment);
        }

        // GET: Komments/Delete/5
        public ActionResult Delete(int? id)
        {
            Komment nowAccount = db.Komments.Find(id);
            if (nowAccount == null || ((Account)Session["Account"]).AccountId != nowAccount.AccountId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Komment komment = db.Komments.Find(id);
            if (komment == null)
            {
                return HttpNotFound();
            }
            return View(komment);
        }

        // POST: Komments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Komment komment = db.Komments.Find(id);
            db.Komments.Remove(komment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
