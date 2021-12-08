
using System.ComponentModel.DataAnnotations;

namespace ScheduleTask.Models.Task
{
    public class AddTaskViewModel
    {
        [Required(ErrorMessage = "وارد کردن نام کار جدید اجباری است")]
        [MinLength(3,ErrorMessage = "فیلد نام کار باید بیشتر از 3 کاراکتر باشد")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "وارد کردن فیلد دقیقه اجباری است")]
        [Range(0,59,ErrorMessage = "فیلد دقیقه باید بین 0 تا 59 باشد")]
        public int Minute { get; set; }
        
        [Required(ErrorMessage = "وارد کردن فیلد ساعت اجباری است")]
        [Range(0,24,ErrorMessage = "فیلد ساعت باید بین 0 تا 24 باشد")]
        public int Hour { get; set; }
    }
}

/*
//
*/