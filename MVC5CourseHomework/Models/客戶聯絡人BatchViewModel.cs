using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5CourseHomework.Models
{
    public class 客戶聯絡人BatchViewModel
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 職稱 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [RegularExpression(@"^09\d{2}-\d{6}", ErrorMessage = "你輸入的格式不正確，格式為 09 開頭加 8 碼數字")]
        public string 手機 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [RegularExpression(@"^[0]\d{8,9}", ErrorMessage = "你輸入的格式不正確，格式為 0 開頭加 8 或 9 碼數字")]
        public string 電話 { get; set; }
    }
}