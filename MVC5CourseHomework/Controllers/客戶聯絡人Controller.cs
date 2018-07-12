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

        public ActionResult Search(string contantName, string contantPhone, string contantTel, string contantTitle)
        {
            var data = custContantRepo.Search(contantName, contantPhone, contantTel, contantTitle);
            var titleData = custContantRepo.GetContantTitle();
            ViewBag.contantTitle = new SelectList(titleData);
            return View("Index", data);
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



        public ActionResult Export(string contantName, string contantPhone, string contantTel, string contantTitle)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                int timeStamp = Convert.ToInt32(DateTime.UtcNow.AddHours(8).Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

                var data = custContantRepo
                    .Search(contantName, contantPhone, contantTel, contantTitle)
                    .Select(s => new { s.Id, s.職稱, s.姓名, s.Email, s.手機, s.電話, s.客戶資料.客戶名稱 });

                var ws = wb.Worksheets.Add("cusdata", 1);

                ws.Cell("A1").Value = "Id";
                ws.Cell("B1").Value = "職稱";
                ws.Cell("C1").Value = "姓名";
                ws.Cell("D1").Value = "Email";
                ws.Cell("E1").Value = "手機";
                ws.Cell("F1").Value = "電話";
                ws.Cell("G1").Value = "客戶名稱";

                ws.Cell(2, 1).InsertData(data);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return File(
                        memoryStream.ToArray(),
                        "application/vnd.ms-excel",
                        $"Export_客戶資料_{timeStamp}.xlsx");
                }
            }
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
