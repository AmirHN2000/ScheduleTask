
using ScheduleTask.Models.Time;

namespace ScheduleTask.Models.Task
{
    public class ShowTaskViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Time { get; set; }
        public bool Status { get; set; }
        public bool Selected { get; set; }
    }
}