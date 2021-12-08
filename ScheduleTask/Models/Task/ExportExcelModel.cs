using EPPlus.Core.Extensions.Attributes;

namespace ScheduleTask.Models.Task
{
    [ExcelWorksheet("Tasks")]
    public class ExportExcelModel
    {
        [ExcelTableColumn(columnName:"نام کاربر")]
        public string FullNameUser { get; set; }
        
        [ExcelTableColumn(columnName:"نام کار")]
        public string Title { get; set; }
        
        [ExcelTableColumn(columnName:"وضعیت")]
        public string Status { get; set; }
        
        [ExcelTableColumn(columnName:"زمان تقریبی انجام کار")]
        public string Time { get; set; }
        
        [ExcelTableColumn(columnName:"تاریخ ایجاد کار")]
        public string CreateDate { get; set; }

        [ExcelTableColumn(columnName:"تاریخ انجام کار")]
        public string FinishDate { get; set; }
    }
}