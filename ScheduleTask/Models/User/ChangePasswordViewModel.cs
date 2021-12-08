using System.ComponentModel.DataAnnotations;

namespace ScheduleTask.Models.User
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        
        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "وارد کردن فیلد {0} اجباری است")]
        [MinLength(6,ErrorMessage = "قیلد {0} باید بیشتر از 6 کاراکنر باشد")]
        public string NewPassword { get; set; }
    }
}