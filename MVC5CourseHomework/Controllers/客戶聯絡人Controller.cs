using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MVC5CourseHomework.Helpers;
using MVC5CourseHomework.Models;

namespace MVC5CourseHomework.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        客戶聯絡人Repository custContantRepo;
        客戶資料Repository customerRepo;

        public 客戶聯絡人Controller()
        {
            custContantRepo = RepositoryHelper.Get客戶聯絡人Repository();
            customerRepo = RepositoryHelper.Get客戶資料Repository(custContantRepo.UnitOfWork);
        }

        // GET: 客戶聯絡人
        public ActionResult Index()
        {
            var titleData = custContantRepo.GetContantTitle();
            ViewBag.contantTitle = new SelectList(titleData);

            var data = custContantRepo.All().ToList();
            return View(data);
        }

        public ActionResult Search(string submit, string contantName, string contantPhone, string contantTel, string contantTitle)
        {
            var data = custContantRepo.Search(contantName, contantPhone, contantTel, contantTitle);
            var titleData = custContantRepo.GetContantTitle();
            ViewBag.contantTitle = new SelectList(titleData);

            if (submit.Equals("Search"))
            {
                return View("Index", data);
            }
            else
            {
                int timeStamp = Convert.ToInt32(DateTime.UtcNow.AddHours(8).Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                ExcelExportHelper hepler = new ExcelExportHelper();

                var exportData =
                    data.Select(s => new 客戶聯絡人ViewModel()
                    {
                        Id = s.Id,
                        職稱 = s.職稱,
                        姓名 = s.姓名,
                        Email = s.Email,
                        手機 = s.手機,
                        電話 = s.電話,
                        客戶名稱 = s.客戶資料.客戶名稱
                    })
                    .ToList();

                var memoryStream = hepler.Stream(exportData);

                return File(
                    memoryStream.ToArray(),
                    "application/vnd.ms-excel",
                    $"Export_客戶聯絡人_{timeStamp}.xlsx");
            }
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = custContantRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            var 客戶資料 = customerRepo.All();
            ViewBag.客戶Id = new SelectList(客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                custContantRepo.Add(客戶聯絡人);
                custContantRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            var 客戶資料 = customerRepo.All();
            ViewBag.客戶Id = new SelectList(客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = custContantRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }

            var 客戶資料 = customerRepo.All();
            ViewBag.客戶Id = new SelectList(客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = custContantRepo.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                custContantRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            var 客戶資料 = customerRepo.All();
            ViewBag.客戶Id = new SelectList(客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = custContantRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = custContantRepo.Find(id);
            custContantRepo.Delete(客戶聯絡人);
            custContantRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                custContantRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
