using System.ComponentModel.DataAnnotations;
using ScheduleTask.Models.Time;

namespace ScheduleTask.Models.Task
{
    public class EditTaskViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "نام کار")]
        [Required(ErrorMessage = "وارد کردن فیلد {0} اجباری است")]
        [MinLength(3,ErrorMessage = "فیلد {0} باید بیشتر از 3 کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "وضعیت کار")]
        public int Status { get; set; }
        
        [Display(Name = "ساعت")]
        [Required(ErrorMessage = "وارد کردن فیلد {0} اجباری است")]
        [Range(0,24,ErrorMessage = "فیلد ساعت باید بین 0 تا 24 باشد")]
        public byte Hour { get; set; }
        
        [Display(Name = "دقیقه")]
        [Required(ErrorMessage = "وارد کردن فیلد {0} اجباری است")]
        [Range(0,59,ErrorMessage = "فیلد دقیقه باید بین 0 تا 59 باشد")]
        public byte Minute { get; set; }

        public string PageBack { get; set; }
        public bool InableEditTask { get; set; } = true;
    }
}