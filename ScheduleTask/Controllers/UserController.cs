using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DNTCaptcha.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScheduleTask.Data;
using ScheduleTask.Models.User;
using ScheduleTask.Services;
using ScheduleTask.Utils;
using SmartBreadcrumbs.Attributes;

namespace ScheduleTask.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IWebHostEnvironment _environment;
        private readonly SignInManager<User> _signInManager;
        private readonly IDNTCaptchaValidatorService _captchaValidatorService;

        public UserController(UserService userService,
            IWebHostEnvironment environment, SignInManager<User> signInManager, IDNTCaptchaValidatorService
                captchaValidatorService)
        {
            _userService = userService;
            _environment = environment;
            _signInManager = signInManager;
            _captchaValidatorService = captchaValidatorService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new {status=false, message=ModelState.GetErrorFromModelError()});
            
            var user = await _userService.FindByNameAsync(model.UserName);
            if (user == null)
                return Json(new {status= false, message=new []{"نام کاربری یا رمز عبور صحیح نمی باشد"}});

            if (!_captchaValidatorService.HasRequestValidCaptchaEntry(Language.Persian, DisplayMode.ShowDigits))
                return Json(new {status= false, message=new []{"لطفا کد امنیتی را به طور صحیح وارد کنید."}});

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRememberMe
                ,true);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Json(new {status=true, link=model.ReturnUrl});
                }
                
                return Json(new {status=true, link="/Home/Dashboard"});
            }

            if(result.IsLockedOut)
                return Json(new {status= false,message=new []{
                    "بدلیل 5 تلاش ناموفق در ورود به حساب کاربری, حساب کاربری شما بمدت 15 دقیقه مسدود شده است"}});
            
            return Json(new {status= false,message=new []{ "نام کاربری یا رمز عبور صحیح نمی باشد"}});
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Breadcrumb("افزودن کاربر جدید", FromAction = "Dashboard", FromController = typeof(HomeController))]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.IsExistMember(model.Name, model.Family))
                {
                    return Json(new {status = false, message =new[]{ "این کاربر قبلا اضافه شده است"}});
                }

                var user = new User()
                {
                    Name = model.Name,
                    Family = model.Family,
                    UserName = model.UserName,
                };
                var resultAdd = await _userService.CreateAsync(user, model.Password);
                if (resultAdd.Succeeded)
                {
                    await _userService.AddToRoleAsync(user,"User");
                    if (model.Image!=null)
                    {
                        await SaveImage(model.Image, user.Id);
                    }
                    return Json(new {status = true, message = "کاربر با موفقیت ایجاد شد"});
                }

                List<string> errors = new List<string>();
                foreach (var resultAddError in resultAdd.Errors)
                    errors.Add(resultAddError.Description);
                return Json(new {status = false, message = errors});
            }

            return Json(new {status = false, message =ModelState.GetErrorFromModelError()});
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        [Breadcrumb("ویرایش حساب کاربری", FromAction = "Dashboard", FromController = typeof(HomeController))]
        public async Task<IActionResult> UpdateUser(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = await _userService.FindByIdAsync(id);
                if (user==null) return NotFound();
                
                var model = user.Adapt<UpdateUserViewModel>();
                var path=GetPath(id);
                if (System.IO.File.Exists(path))
                {
                    model.ExistImage = true;
                }
                
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new{status=false, message=ModelState.GetErrorFromModelError()});
            }
            
            
            
            var user =await _userService.FindByIdAsync(model.Id);
            if (User.IsInRole("Admin"))
            {
                user.Name = model.Name;
                user.Family = model.Family;
                user.UserName = model.UserName;
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var resultR=await _userService.RemovePasswordAsync(user);
                    var resultA=await _userService.AddPasswordAsync(user, model.NewPassword);
                    if (!resultR.Succeeded || !resultA.Succeeded)
                    {
                        List<string> errors = new List<string>();
                        foreach (var resultAddError in resultA.Errors)
                            errors.Add(resultAddError.Description);
                        foreach (var resultAddError in resultR.Errors)
                            errors.Add(resultAddError.Description);
                        return Json(new {status = false, message = errors});
                    }
                }
            }
                
            var result = await _userService.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (model.Image!=null)
                    await SaveImage(model.Image, model.Id);
                else if (model.ExistImage && model.DeleteImage)
                    DeleteImage(model.Id);
                
                return Json(new {status = true, message = "کاربر با موفقیت ویرایش شد"});
            }

            return Json(new {status = false, message = ModelState.GetErrorFromModelError()});
        }

        private string GetPath(string id)
        {
            var name = id + ".jpg";
            var basePath = _environment.WebRootPath;
            return Path.Combine(basePath, "Images",name);
        }
        private async Task SaveImage(IFormFile image, string id)
        {
            var path=GetPath(id);
            await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            await image.CopyToAsync(stream);
            //await stream.FlushAsync();
        }

        [AllowAnonymous]
        public IActionResult ShowImage(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var path=GetPath(id);
            if (System.IO.File.Exists(path))
            {
                return PhysicalFile(path, "image/jpg");
            }

            var defaultPath = GetPath("NoImage");
            return PhysicalFile(defaultPath, "image/jpg");
        }

        private void DeleteImage(string id)
        {
            var path = GetPath(id);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        
        [Authorize(Roles = "Admin")]
        [Breadcrumb("نمایش کاربران", FromAction = "Dashboard", FromController = typeof(HomeController))]
        public IActionResult ShowUsers()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers(int start, int length)
        {
            var filter = HttpContext.Request.Form["search[Value]"].ToString();
            var model =await _userService.GetAllUser();

            var totalCount = model.Count;
            var filterCount = totalCount;
            
            if (!string.IsNullOrEmpty(filter))
            {
                model = model.Where(x =>
                    x.Name.Contains(filter) || x.Family.Contains(filter) || x.UserName.Contains(filter)).ToList();
                filterCount = model.Count;
            }

            var result = model
                .OrderBy(x => x.Family)
                .ThenBy(x => x.Name)
                .Select((x,i)=>new
                {
                    number=i+1,
                    x.Id,
                    x.UserName,
                    x.Name,
                    x.Family
                })
                .Skip(start)
                .Take(length)
                .ToList();
            
            return Json(new {data = result, recordsTotal=totalCount, recordsFiltered=filterCount});
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = await _userService.FindByIdAsync(id);
                var result = await _userService.DeleteAsync(user);
                if (result.Succeeded)
                {
                    DeleteImage(id);
                    return Json(new {status = true});
                }

                var error = result.Errors.First();
                return Json(new {status = false, message = error.Description});
            }

            return Json(new {status = false, message = "خطا در حذف کاربر"});
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}