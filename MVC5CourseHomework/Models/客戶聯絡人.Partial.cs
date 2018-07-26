namespace MVC5CourseHomework.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        // 實作模型驗證
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶聯絡人Repository custContantRepo = RepositoryHelper.Get客戶聯絡人Repository();

            if (custContantRepo.CheckMail(this.Id, this.Email, this.客戶Id))
            {
                yield return new ValidationResult("您輸入的 E-Mail 已重複存在", new string[] { "Email" });
            }
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }

        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }

        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }

        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress(ErrorMessage = "你輸入的 E-Mail 格是不正確")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [RegularExpression(@"^09\d{2}-\d{6}", ErrorMessage = "你輸入的格式不正確，格式為 09XX-XXXXXX ")]
        public string 手機 { get; set; }

        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [RegularExpression(@"^[0]\d{8,9}", ErrorMessage = "你輸入的格式不正確，格式為 0 開頭加 8 或 9 碼數字")]
        public string 電話 { get; set; }
        public bool 是否已刪除 { get; set; }

        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
