using System.ComponentModel.DataAnnotations;

namespace ScheduleTask.Models.User
{
    public class LoginViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "وارد کردن فیلد {0} اجباری است")]
        [MinLength(3,ErrorMessage = "فیلد {0} باید بیشتر از 3 کاراکتر باشد")]
        public string UserName { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "وارد کردن فیلد {0} اجباری است")]
        [MinLength(6,ErrorMessage = "فیلد {0} باید بیشتر از 6 کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool IsRememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}