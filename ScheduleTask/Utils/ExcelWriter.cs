using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using EPPlus.Core.Extensions;
using EPPlus.Core.Extensions.Style;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace ScheduleTask.Utils
{
    public class ExcelWriter
    {
        public async Task<byte[]> GetExcelBytesAsync<T>(List<T> model)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            var excelPakage = model.ToExcelPackage();
            var worksheets=excelPakage.Workbook.Worksheets.First();
            worksheets.Cells.AutoFitColumns();
            worksheets.View.RightToLeft = true;
            worksheets.Tables[0].TableStyle = TableStyles.Medium13;

            for (var row = worksheets.Dimension.Start.Row+1 ; row <= worksheets.Dimension.End.Row; row++)
            {
                var name = "C" + row;
                var cell = worksheets.Cells[name];
                if (cell.Value.Equals("در حال انجام"))
                {
                    cell.SetBackgroundColor(Color.Yellow);
                }
                else
                {
                    cell.SetBackgroundColor(Color.LightGreen);
                }
            }
            
            var bytes =await excelPakage.GetAsByteArrayAsync();
            return bytes;
        }
    }
}