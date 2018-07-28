using MVC5CourseHomework.Helpers;
using MVC5CourseHomework.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using X.PagedList;

namespace MVC5CourseHomework.Controllers
{
    [ShareLog]
    public class 客戶資料Controller : Controller
    {
        CustomerRepository customerRepo;
        View_客戶對應聯絡人及銀行帳戶數量Repository dbViewRepo;
        CustomerContantRepository contantRepo;
        CustomerBankRepository bankRepo;

        public 客戶資料Controller()
        {
            customerRepo = RepositoryHelper.Get客戶資料Repository();
            dbViewRepo = RepositoryHelper.GetView_客戶對應聯絡人及銀行帳戶數量Repository(customerRepo.UnitOfWork);
            contantRepo = RepositoryHelper.Get客戶聯絡人Repository(customerRepo.UnitOfWork);
            bankRepo = RepositoryHelper.Get客戶銀行資訊Repository(customerRepo.UnitOfWork);
        }

        // GET: 客戶資料
        public ActionResult Index(string sortColumn, int? page)
        {
            //string sortColumn = "";
            int intPageSize = 5;

            int intPageNumber = page ?? 1;

            ViewBag.客戶名稱 = sortColumn == "客戶名稱" ? "客戶名稱_desc" : "客戶名稱";
            ViewBag.統一編號 = sortColumn == "統一編號" ? "統一編號_desc" : "統一編號";
            ViewBag.電話 = sortColumn == "電話" ? "電話_desc" : "電話";
            ViewBag.傳真 = sortColumn == "傳真" ? "傳真_desc" : "傳真";
            ViewBag.地址 = sortColumn == "地址" ? "地址_desc" : "地址";
            ViewBag.Email = sortColumn == "Email" ? "Email_desc" : "Email";
            ViewBag.客戶分類 = sortColumn == "客戶分類" ? "客戶分類_desc" : "客戶分類";
            ViewBag.Sort = sortColumn;

            var data = customerRepo.SortBy(sortColumn).ToPagedList(intPageNumber, intPageSize);

            var categoryData = customerRepo.GetCustomerCategory();
            ViewBag.custCategory = new SelectList(categoryData);

            return View(data);
        }

        public ActionResult Search(string submit, string custName, string custUid, string custTel, string custFax, string custCategory)
        {
            var data = customerRepo.Search(custName, custUid, custTel, custFax, custCategory);

            var categoryData = customerRepo.GetCustomerCategory();
            ViewBag.custCategory = new SelectList(categoryData);

            if (submit.Equals("Search"))
            {
                return View("Index", data);
            }
            else
            {
                int timeStamp = Convert.ToInt32(DateTime.UtcNow.AddHours(8).Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                ExcelExportHelper hepler = new ExcelExportHelper();

                var exportData =
                    data.Select(s => new 客戶資料ViewModel()
                    {
                        Id = s.Id,
                        客戶名稱 = s.客戶名稱,
                        統一編號 = s.統一編號,
                        電話 = s.電話,
                        傳真 = s.傳真,
                        地址 = s.地址,
                        Email = s.Email,
                        客戶分類 = s.客戶分類
                    })
                    .ToList();

                var memoryStream = hepler.Stream(exportData);

                return File(
                    memoryStream.ToArray(),
                    "application/vnd.ms-excel",
                    $"Export_客戶資料_{timeStamp}.xlsx");
            }
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                customerRepo.Add(客戶資料);
                customerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = customerRepo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = customerRepo.Find(id);
            customerRepo.Delete(客戶資料);
            customerRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult DbViewList()
        {
            var data = dbViewRepo.All();
            return View(data);
        }

        public ActionResult ViewModelList()
        {
            var contantData = contantRepo.All();
            var bankData = bankRepo.All();
            var data = customerRepo.GetContantBankCount(contantData, bankData);
            return View(data);
        }

        public JsonResult GetCount(int Id)
        {
            var contantData = contantRepo.All();
            var bankData = bankRepo.All();

            var data = customerRepo
                .GetContantBankCount(contantData, bankData)
                .Where(w => w.客戶Id.Equals(Id));

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
