using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5CourseHomework.Models
{   
	public  class View_客戶對應聯絡人及銀行帳戶數量Repository : EFRepository<View_客戶對應聯絡人及銀行帳戶數量>, IView_客戶對應聯絡人及銀行帳戶數量Repository
	{

	}

	public  interface IView_客戶對應聯絡人及銀行帳戶數量Repository : IRepository<View_客戶對應聯絡人及銀行帳戶數量>
	{

	}
}