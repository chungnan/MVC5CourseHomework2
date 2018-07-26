using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5CourseHomework.Models
{

    public class CustomerRepository : EFRepository<客戶資料>, ICustomerRepository
    {
        public CustomerContantRepository ContantRepo { get; set; }

        public CustomerRepository()
        {
            this.ContantRepo = RepositoryHelper.Get客戶聯絡人Repository(base.UnitOfWork);
        }

        public IQueryable<客戶資料> All()
        {
            return base.All().Where(w => w.是否已刪除 == false);
        }

        public IQueryable<客戶資料> SortBy(string column)
        {
            var data = this.All();

            switch (column)
            {
                case "客戶名稱_desc":
                    data = data.OrderByDescending(d => d.客戶名稱);
                    break;
                case "客戶名稱":
                    data = data.OrderBy(o => o.客戶名稱);
                    break;
                case "統一編號_desc":
                    data = data.OrderByDescending(d => d.統一編號);
                    break;
                case "統一編號":
                    data = data.OrderBy(o => o.統一編號);
                    break;
                case "電話_desc":
                    data = data.OrderByDescending(d => d.電話);
                    break;
                case "電話":
                    data = data.OrderBy(o => o.電話);
                    break;
                case "傳真_desc":
                    data = data.OrderByDescending(d => d.傳真);
                    break;
                case "傳真":
                    data = data.OrderBy(o => o.傳真);
                    break;
                case "地址_desc":
                    data = data.OrderByDescending(d => d.地址);
                    break;
                case "地址":
                    data = data.OrderBy(o => o.地址);
                    break;
                case "Email_desc":
                    data = data.OrderByDescending(d => d.Email);
                    break;
                case "Email":
                    data = data.OrderBy(o => o.Email);
                    break;
                case "客戶分類_desc":
                    data = data.OrderByDescending(d => d.客戶分類);
                    break;
                case "客戶分類":
                    data = data.OrderBy(o => o.客戶分類);
                    break;
                default:
                    data = data.OrderBy(s => s.Id);
                    break;
            }

            return data;
        }

        internal IQueryable<客戶資料> Search(string custName, string custUid, string custTel, string custFax, string custCategory)
        {
            var data = this.All();

            if (!string.IsNullOrEmpty(custName))
                data = data.Where(w => w.客戶名稱.Contains(custName));

            if (!string.IsNullOrEmpty(custUid))
                data = data.Where(w => w.統一編號.Contains(custUid));

            if (!string.IsNullOrEmpty(custTel))
                data = data.Where(w => w.電話.Contains(custTel));

            if (!string.IsNullOrEmpty(custFax))
                data = data.Where(w => w.傳真.Contains(custFax));

            if (!string.IsNullOrEmpty(custCategory))
                data = data.Where(w => w.客戶分類.Equals(custCategory));

            return data;
        }

        internal 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(f => f.Id == id);
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }

        public IQueryable<string> GetCustomerCategory()
        {
            return this.All().Select(s => s.客戶分類).Distinct();
        }

        internal IQueryable<客戶對應聯絡人及銀行帳戶數量ViewModel> GetContantBankCount(
            IQueryable<客戶聯絡人> contantData, IQueryable<客戶銀行資訊> bankData)
        {
            var data = (
                from customer in this.All()
                select new 客戶對應聯絡人及銀行帳戶數量ViewModel()
                {
                    客戶Id = customer.Id,
                    客戶名稱 = customer.客戶名稱,
                    聯絡人數量 = contantData.Where(w => w.客戶Id.Equals(customer.Id)).Count(),
                    銀行帳戶數量 = bankData.Where(w => w.客戶Id.Equals(customer.Id)).Count()
                })
                .Take(10);

            return data;
        }
    }

    public interface ICustomerRepository : IRepository<客戶資料>
    {

    }
}