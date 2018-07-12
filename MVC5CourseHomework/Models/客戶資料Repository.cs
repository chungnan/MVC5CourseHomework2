using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5CourseHomework.Models
{
    public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public IQueryable<客戶資料> All()
        {
            return base.All().Where(w => w.是否已刪除 == false);
        }

        internal IQueryable<客戶資料> Search(string custName, string custUid, string custTel, string custFax)
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
    }

    public  interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}