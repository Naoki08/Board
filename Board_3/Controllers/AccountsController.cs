using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Board_3.Models;

namespace Board_3.Controllers
{
    public class AccountsController : Controller
    {
        private BDBContext db = new BDBContext();

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(db.Accounts.Find(id).AccountId != ((Account)Session["Account"]).AccountId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountId,Name,PassWord")] Account account)
        {
            if (ModelState.IsValid) { 
            
                if(db.Accounts.Any(a => a.Name == account.Name) == true)
                {
                    ViewBag.text = "存在しているアカウント名です";
                    return View();
                }
                db.Accounts.Add(account);
                db.SaveChanges();
                Session["Account"] = account;
                return RedirectToAction("Index", "Komments");
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountId,Name,PassWord")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(id != ((Account)Session["Account"]).AccountId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login([Bind(Include ="Name,PassWord")] Account account)
        {
            if(string.IsNullOrEmpty(account.Name) || string.IsNullOrEmpty(account.PassWord))
            {
                return View(account);
            }
            if(!db.Accounts.Any(a => a.Name == account.Name))
            {
                return View(account);
            }

            var nowAccount = db.Accounts.SingleOrDefault(a => a.Name == account.Name);

            if(nowAccount == null)
            {
                return View(account);
            }

            if(nowAccount.PassWord != account.PassWord)
            {
                return View(account);
            }
            Session["Account"] = nowAccount;

            return RedirectToAction("Index", "Komments");
        }

        public ActionResult Logout()
        {
            Session["Account"] = null;
            return RedirectToAction("Login", "Accounts");
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
