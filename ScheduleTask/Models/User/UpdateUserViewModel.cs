using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ScheduleTask.Attributes;

namespace ScheduleTask.Models.User
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "وارد کزدن فیلد {0} اجباری است")]
        [Display(Name = "نام")]
        [MinLength(3,ErrorMessage= "تعداد کاراکتر های فیلد {0} نباید کمتر از سه کاراکتر باشد")]
        [MaxLength(80,ErrorMessage = "تعداد کاراکتر های فیلد {0} نباید بیش از 80 کاراکتر باشد")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "وارد کزدن فیلد {0} اجباری است")]
        [Display(Name = "نام خانوادگی")]
        [MinLength(3,ErrorMessage= "تعداد کاراکتر های فیلد {0} نباید کمتر از سه کاراکتر باشد")]
        [MaxLength(80,ErrorMessage = "تعداد کاراکتر های فیلد {0} نباید بیش از 80 کاراکتر باشد")]
        public string Family { get; set; }
        
        [Required(ErrorMessage = "وارد کزدن فیلد {0} اجباری است")]
        [Display(Name = "نام کاربری")]
        [MinLength(3,ErrorMessage= "تعداد کاراکتر های فیلد {0} نباید کمتر از سه کاراکتر باشد")]
        [MaxLength(80,ErrorMessage = "تعداد کاراکتر های فیلد {0} نباید بیش از 80 کاراکتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "عکس حساب کاربری")]
        [ImageValidation(1)]
        public IFormFile Image { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [MinLength(6,ErrorMessage = "فیلد {0} باید بیشتر از 6 کاراکتر باشد")]
        public string NewPassword { get; set; }

        public bool ExistImage { get; set; }
        public bool DeleteImage { get; set; }
    }
}