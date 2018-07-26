using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5CourseHomework.Models
{
    public  class CustomerContantRepository : EFRepository<客戶聯絡人>, ICustomerContantRepository
    {
        public IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(w => w.是否已刪除 == false);
        }

        public IQueryable<客戶聯絡人> SortBy(string column)
        {
            var data = this.All();

            switch (column)
            {
                case "職稱_desc":
                    data = data.OrderByDescending(d => d.職稱);
                    break;
                case "職稱":
                    data = data.OrderBy(o => o.職稱);
                    break;
                case "姓名_desc":
                    data = data.OrderByDescending(d => d.姓名);
                    break;
                case "姓名":
                    data = data.OrderBy(o => o.姓名);
                    break;
                case "Email_desc":
                    data = data.OrderByDescending(d => d.Email);
                    break;
                case "Email":
                    data = data.OrderBy(o => o.Email);
                    break;
                case "手機_desc":
                    data = data.OrderByDescending(d => d.手機);
                    break;
                case "手機":
                    data = data.OrderBy(o => o.手機);
                    break;
                case "電話_desc":
                    data = data.OrderByDescending(d => d.電話);
                    break;
                case "電話":
                    data = data.OrderBy(o => o.電話);
                    break;
                case "客戶名稱_desc":
                    data = data.OrderByDescending(d => d.客戶資料.客戶名稱);
                    break;
                case "客戶名稱":
                    data = data.OrderBy(o => o.客戶資料.客戶名稱);
                    break;
                default:
                    data = data.OrderBy(o => o.Id);
                    break;
            }


            return data;
        }

        internal IQueryable<客戶聯絡人> Search(string contantName, string contantPhone, string contantTel, string contantTitle)
        {
            var data = this.All();

            if (!string.IsNullOrEmpty(contantName))
                data = data.Where(w => w.姓名.Contains(contantName));

            if (!string.IsNullOrEmpty(contantPhone))
                data = data.Where(w => w.手機.Contains(contantPhone));

            if (!string.IsNullOrEmpty(contantTel))
                data = data.Where(w => w.電話.Contains(contantTel));

            if (!string.IsNullOrEmpty(contantTitle))
                data = data.Where(w => w.職稱.Equals(contantTitle));

            return data;
        }

        internal 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(f => f.Id == id);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

        internal bool CheckMail(int id, string email, int 客戶id)
        {
            bool boolResult = false;

            if (id == 0)
            {
                // 新增時檢查 E-Mail 是否重複
                boolResult = this.All().Any(w => w.Email.Equals(email) && w.客戶Id.Equals(客戶id));
            }
            else
            {
                // 編輯時檢查 E-Mail 是否重複，需排除本身 id
                boolResult = this.All().Any(w => w.Email.Equals(email) && w.客戶Id.Equals(客戶id) & w.Id != id);
            }

            return boolResult;
        }

        public IQueryable<string> GetContantTitle()
        {
            return this.All().Select(s => s.職稱 ).Distinct();
        }
    }

    public  interface ICustomerContantRepository : IRepository<客戶聯絡人>
    {

    }
}