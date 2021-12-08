using ScheduleTask.Data;

namespace ScheduleTask.Models.Task
{
    public class ShowTaskPartial
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public TaskStatus Status { get; set; }
        public string CreateDate { get; set; }
        public string ModifyDate { get; set; }
        public string FinishDate { get; set; }
        public string Time { get; set; }
    }
}