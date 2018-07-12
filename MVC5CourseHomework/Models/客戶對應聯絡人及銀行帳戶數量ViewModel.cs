using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC5CourseHomework.Models
{
    public class 客戶對應聯絡人及銀行帳戶數量ViewModel
    {
        public int 客戶Id { get; set; }

        public string 客戶名稱 { get; set; }

        public int 聯絡人數量 { get; set; }

        public int 銀行帳戶數量 { get; set; }
    }
}