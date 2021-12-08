using System.ComponentModel.DataAnnotations;

namespace ScheduleTask.Data
{
    public enum TaskStatus
    {
        [Display(Name = "در حال انجام")]
        Pending,
        [Display(Name = "انجام شده")]
        Done
    }
}