using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleTask.Data;
using ScheduleTask.Interface;
using ScheduleTask.Models.Time;
using ScheduleTask.Utils;
using SmartBreadcrumbs.Attributes;

namespace ScheduleTask.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TimeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly INecessaryTime _timeService;

        public TimeController(AppDbContext dbContext, INecessaryTime timeService)
        {
            _dbContext = dbContext;
            _timeService = timeService;
        }

        [HttpGet]
        [Breadcrumb("تغییر محدوده زمان", FromAction = "Dashboard", FromController = typeof(HomeController))]
        public async Task<IActionResult> EditTime()
        {
            var time = await _timeService.GetTime(TimeName.LimitedTimeInsertTask);
            var model = new EditTimeViewModel()
            {
                Name = time.Name,
                Hour = time.Hour,
                Minute = time.Minute
            };
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTime(EditTimeViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new {status=false, message=ModelState.GetErrorFromModelError()});
            
            var time = await _timeService.GetTime(model.Name);
            if (time==null)
            {
                return NotFound();
            }

            if (model.Minute==0 && model.Hour==0)
            {
                return Json(new
                {
                    status = false,
                    message = new[]{"شما نمی توانید ساعت 00:00 را وارد کنید، به جای آن می توانید ار ساعت 24:00 استفاده کنید"}
                });
            }

            if (model.Hour>=24 && model.Minute!=0)
            {
                return Json(new
                {
                    status = false,
                    message = new[]{"ساعت وارد شده نامعتبر است"}
                });
            }

            time.Hour = model.Hour;
            time.Minute = model.Minute;
            await _dbContext.SaveChangesAsync();
            return Json(new {status=true, message="محدوده زمانی با موفقیت تغییر کرد"});
        }
    }
}