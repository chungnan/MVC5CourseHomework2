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
    public class 客戶銀行資訊Controller : Controller
    {
        客戶銀行資訊Repository custBankRepo;
        客戶資料Repository customerRepo;

        public 客戶銀行資訊Controller()
        {
            custBankRepo = RepositoryHelper.Get客戶銀行資訊Repository();
            customerRepo = RepositoryHelper.Get客戶資料Repository(custBankRepo.UnitOfWork);
        }

        private Customers db = new Customers();

        // GET: 客戶銀行資訊
        public ActionResult Index()
        {
            var data = custBankRepo.All();
            return View(data);
        }

        public ActionResult Search(string bankName)
        {
            var data = custBankRepo.Search(bankName);
            return View("Index", data);
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = custBankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            var 客戶資料 = customerRepo.All();
            ViewBag.客戶Id = new SelectList(客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                custBankRepo.Add(客戶銀行資訊);
                custBankRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            var 客戶資料 = customerRepo.All();
            ViewBag.客戶Id = new SelectList(客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = custBankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }

            var 客戶資料 = customerRepo.All();
            ViewBag.客戶Id = new SelectList(客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db = custBankRepo.UnitOfWork.Context;
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var 客戶資料 = customerRepo.All();
            ViewBag.客戶Id = new SelectList(客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = custBankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = custBankRepo.Find(id);
            custBankRepo.Delete(客戶銀行資訊);
            custBankRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Export(string bankName)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                int timeStamp = Convert.ToInt32(DateTime.UtcNow.AddHours(8).Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

                var data = custBankRepo
                    .Search(bankName)
                    .Select(s => new { s.Id, s.銀行名稱, s.銀行代碼, s.分行代碼, s.帳戶名稱, s.帳戶號碼, s.客戶資料.客戶名稱 });

                var ws = wb.Worksheets.Add("cusdata", 1);

                ws.Cell("A1").Value = "Id";
                ws.Cell("B1").Value = "銀行名稱";
                ws.Cell("C1").Value = "銀行代碼";
                ws.Cell("D1").Value = "分行代碼";
                ws.Cell("E1").Value = "帳戶名稱";
                ws.Cell("F1").Value = "帳戶號碼";
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
                custBankRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
