using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;

namespace MVC5CourseHomework.Helpers
{
    public class ExcelExportHelper
    {
        public XLWorkbook Export<T>(List<T> data)
        {
            // 建立 Excel 物件
            XLWorkbook workbook = new XLWorkbook();

            // 加入 Excel Sheet 名稱
            var sheet = workbook.Worksheets.Add("Report");

            // 標頭首欄位置
            int colIdx = 1;

            // 透過迴圈取得 Propertie 名稱
            foreach (var item in typeof(T).GetProperties())
            {
                sheet.Cell(1, colIdx++).Value = item.Name;
            }

            int rowIdx = 2;
            foreach (var item in data)
            {
                //每筆資料欄位起始位置
                int conlumnIndex = 1;
                foreach (var jtem in item.GetType().GetProperties())
                {
                    //將資料內容加上 "'" 避免受到 excel 預設格式影響，並依 row 及 column 填入
                    sheet.Cell(rowIdx, conlumnIndex).Value = string.Concat("'", Convert.ToString(jtem.GetValue(item, null)));
                    conlumnIndex++;
                }
                rowIdx++;
            }
            return workbook;
        }

        public MemoryStream Stream<T>(List<T> data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var workbook = this.Export(data);

                workbook.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
        }
    }
}