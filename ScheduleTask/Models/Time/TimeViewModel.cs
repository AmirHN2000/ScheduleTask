using System.ComponentModel.DataAnnotations;

namespace ScheduleTask.Models.Time
{
    public class TimeViewModel
    {
        [Display(Name = "ساعت")]
        [Required(ErrorMessage = "وارد کردن فیلد {0} اجباری است")]
        [Range(0,24,ErrorMessage = "فیلد ساعت باید بین 0 تا 24 باشد")]
        public byte Hour { get; set; }
        
        [Display(Name = "دقیقه")]
        [Required(ErrorMessage = "وارد کردن فیلد {0} اجباری است")]
        [Range(0,59,ErrorMessage = "فیلد دقیقه باید بین 0 تا 59 باشد")]
        public byte Minute { get; set; }
    }
}