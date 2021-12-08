using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScheduleTask.Data;
using ScheduleTask.Interface;
using ScheduleTask.Models.Task;
using ScheduleTask.Services;
using ScheduleTask.Utils;
using SmartBreadcrumbs.Attributes;
using TaskStatus = ScheduleTask.Data.TaskStatus;

namespace ScheduleTask.Controllers
{
    public class TaskController:Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ITaskService _taskService;
        private readonly INecessaryTime _necessaryTime;
        private readonly UserService _userService;

        public TaskController(AppDbContext dbContext, ITaskService taskService, INecessaryTime necessaryTime, UserService userService)
        {
            _dbContext = dbContext;
            _taskService = taskService;
            _necessaryTime = necessaryTime;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        [Breadcrumb("افزودن کار جدید", FromAction = "Dashboard", FromController = typeof(HomeController))]
        public async Task<IActionResult> NewTask()
        {
            var models = await _taskService.GetAllTasks(User.GetUserId(), DateTime.Now);
            var result = models.OrderByDescending(x => x.CreateDate)
                .Select((x,i) => new ShowTaskViewModel()
                {
                    Id = x.Id,
                    Number = i+1,
                    Status = Convert.ToBoolean((int)x.Status),
                    Title = x.Title,
                    Time = Time.GetTimeString(x.Hour, x.Minute)
                }).ToList();
            
            return View(result);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewTask(List<ShowTaskViewModel> models, int toStatus)
        {
            if (models.Count == 0) return BadRequest();

            foreach (var model in models)
            {
                if (model.Selected)
                {
                    if (model.Status == Convert.ToBoolean(toStatus)) continue;
                
                    var status = (TaskStatus)toStatus;
                    var task = await _taskService.GetTask(model.Id);
                    task.ModifyDate=DateTime.Now;
                    if (status == TaskStatus.Done)
                        task.FinishDate = DateTime.Now;
                    else
                        task.FinishDate = null;
                        
                    task.Status = status;
                }
            }

            await _dbContext.SaveChangesAsync();
            return Json(new {status = true});
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTask(AddTaskViewModel model) {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = ModelState.GetErrorFromModelError() });
            if ((model.Minute==0 && model.Hour==0) || model.Hour>=24)
                return Json(new
                {
                    status = false,
                    message = new[]{"زمان وارد شده نامعتبر است"}
                });

            var time = await _necessaryTime.GetTime(TimeName.LimitedTimeInsertTask);
            var timeLocal = new NecessaryTime()
            {
                Hour = (byte)DateTime.Now.Hour,
                Minute = (byte)DateTime.Now.Minute
            };
            if (time.CompareTime(timeLocal)==-1)
                return Json(new {status=false,message=new[]{ $"شما نمی توانید بعد از ساعت {time.ToString()} کار جدید اضافه کنید" }});
            
            var exist = await _taskService.IsExistTaskInNow(model.Title, User.GetUserId());
            if (exist)
                return Json(new {status=false,message=new[]{"کاری با این نام در امروز وجود دارد"}});
            
            var task = new NewTask()
            {
                Title = model.Title,
                Status = TaskStatus.Pending,
                UserId = User.GetUserId(),
                Hour = (byte)model.Hour,
                Minute = (byte)model.Minute
            };
            await _taskService.AddTask(task);
            var rows =await _dbContext.SaveChangesAsync();

            if (rows>0)
                return Json(new {status=true});
            
            return Json(new {status=false,message=new[]{"خطا در افزودن کار جدید"}});
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await _taskService.GetTask(taskId);
            if (task.CreateDate.CompareDate(DateTime.Now)==-1)
            {
                return Json(new {status = false, message = "این کار مربوط به روزهای قبل است, شما نمی توانید آن را حذف کنید"});
            }
            
            _dbContext.Tasks.Remove(task);
            var rows=await _dbContext.SaveChangesAsync();
            return rows <= 0 ? Json(new {status = false, message = "خطا در حذف کار"}) : Json(new {status=true});
        }
        
        [HttpGet]
        [Authorize(Roles = "User")]
        [Breadcrumb("نمایش کارها", FromAction = "Dashboard", FromController = typeof(HomeController))]
        public IActionResult ShowTasks(int status=-1)
        {
            return View(status);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        [Breadcrumb("ویرایش کار", FromAction = "ShowTasks", FromController = typeof(TaskController))]
        public async Task<IActionResult> EditTask(int id, string pageBack)
        {
            var task = await _taskService.GetTask(id);
            if (task==null)
            {
                return NotFound();
            }

            var model = task.Adapt<EditTaskViewModel>();
            
            if (task.CreateDate.CompareDate(DateTime.Now)==-1)
            {
                ModelState.AddModelError("", "این کار مربوط به روزهای قبل است, شما نمی توانید آن را ویرایش کنید");
                model.InableEditTask = false;
            }

            model.PageBack = "showTasks";
            if (!string.IsNullOrEmpty(pageBack))
                model.PageBack = pageBack;
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(EditTaskViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new {status=false, message=ModelState.GetErrorFromModelError()});
            if ((model.Minute==0 && model.Hour==0) || model.Hour>=24)
                return Json(new
                {
                    status = false,
                    message = new[]{"زمان وارد شده نامعتبر است"}
                });
            
            var task = await _taskService.GetTask(model.Id);
            if (task==null)
            {
                return Json(new {status=false, message=new []{"خطا در ویرایش کار"}});
            }

            if (task.CreateDate.CompareDate(DateTime.Now)==-1)
            {
                return Json(new {status=false, message=new[]{"این کار مربوط به روزهای قبل است, شما نمی توانید آن را ویرایش کنید"}});
            }
            
            if (task.Title!=model.Title)
            {
                task.ModifyDate=DateTime.Now;
            }
            task.Title = model.Title;
                
            if ((int)task.Status!=model.Status)
            {
                task.ModifyDate=DateTime.Now;
                if (model.Status==(int)TaskStatus.Done)
                {
                    task.FinishDate=DateTime.Now;
                }
                else
                {
                    task.FinishDate = null;
                }
            }
            task.Status = (TaskStatus) model.Status;
            task.Minute = model.Minute;
            task.Hour = model.Hour;
            await _dbContext.SaveChangesAsync();
            
            return Json(new {status=true, message=model.PageBack});
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetTasks(int start, int length, string dateP, int status=-1)
        {
            var filter = HttpContext.Request.Form["search[Value]"].ToString();
            DateTime? filterDate=null;
            if (!string.IsNullOrEmpty(dateP))
            {
                filterDate = dateP.ConvertPersianToGregorianDate();
            }

            List<NewTask> models;
            if (status!=-1)
            {
                models = await _taskService.GetAllTasks(User.GetUserId(), filterDate, status:(TaskStatus)status);
            }
            else
            {
                models = await _taskService.GetAllTasks(User.GetUserId(), filterDate);
            }
            
            var totalCount = models.Count;
            var filterCount = totalCount;

            if (!string.IsNullOrEmpty(filter))
            {
                models = models.Where(x => x.Title.Contains(filter)).ToList();
                filterCount = models.Count;
            }
            
            var result = models.OrderByDescending(x => x.CreateDate)
                .Select((x, i) => new
                {
                    Parameters = new {x.Id, EnableEdit=x.CreateDate.CompareDate(DateTime.Now)!=-1},
                    Number = i + 1,
                    x.Title,
                    Date = x.CreateDate.ToShortPersianDateString(),
                    x.Status,
                    Time=Time.GetTimeString(x.Hour, x.Minute)
                })
                .Skip(start)
                .Take(length)
                .ToList();
            
            return Json(new {data = result, recordsTotal = totalCount, recordsFiltered = filterCount});
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> ShowTask(int taskId)
        {
            var task = await _taskService.GetTask(taskId);
            var user = await _userService.FindByIdAsync(task.UserId);
            
            var model = new ShowTaskPartial()
            {
                FullName = _userService.GetFullName(user.Id),
                Title = task.Title,
                Status = task.Status,
                Time=Time.GetTimeString(task.Hour,task.Minute),
                CreateDate = task.CreateDate.ToPersianDateTimeString("dd/MMMM/yyyy - HH:mm"),
                ModifyDate = task.ModifyDate.ToPersianDateTimeString("dd/MMMM/yyyy - HH:mm"),
                FinishDate = task.FinishDate.ToPersianDateTimeString("dd/MMMM/yyyy - HH:mm"),
            };
            return PartialView("_ShowTaskPartialView",model);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Breadcrumb("نمایش کارها", FromAction = "Dashboard", FromController = typeof(HomeController))]
        public async Task<IActionResult> ShowTasksAdmin(int status=-1)
        {
            var users = await _userService.GetAllUser();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var user in users)
            {
                items.Add(new SelectListItem(_userService.GetFullName(user.Id),user.Id));
            }

            ViewData["Users"] = items;
            return View(status);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Breadcrumb("نمایش کارهای کاربر", FromAction = "ShowUsers", FromController = typeof(UserController))]
        public async Task<IActionResult> ShowTasksWithReport(string id)
        {
            ViewData["userId"] = id;
            var model = await _taskService.ReportTasks(id);
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTasksAdmin(int start, int length, string dateP, string userId,
            string fromDate, string toDate, int status=-1)
        {
            var filter = HttpContext.Request.Form["search[Value]"].ToString();
            DateTime? filterDate=null;
            DateTime? fromDateFilter=null;
            DateTime? toDateFilter=null;
            if (!string.IsNullOrEmpty(dateP) && string.IsNullOrEmpty(toDate))
            {
                filterDate = dateP.ConvertPersianToGregorianDate();
            }
            else if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDateFilter = fromDate.ConvertPersianToGregorianDate();
                toDateFilter = toDate.ConvertPersianToGregorianDate();
            }
        
            List<NewTask> models;
            if (!string.IsNullOrEmpty(userId) && userId!="-1")
            {
                if (status!=-1)
                {
                    models = await _taskService.GetAllTasks(userId,filterDate,fromDate:fromDateFilter,
                        toDateFilter, status:(TaskStatus)status);
                }
                else
                {
                    models = await _taskService.GetAllTasks(userId, filterDate, fromDateFilter, toDateFilter);
                }
            }
            else
            {
                if (status!=-1)
                {
                    models = await _taskService.GetAllTasks(date:filterDate, fromDate:fromDateFilter, toDate:toDateFilter,
                        status:(TaskStatus)status);
                }
                else
                {
                    models = await _taskService.GetAllTasks(date:filterDate, fromDate:fromDateFilter, toDate:toDateFilter);
                }
            }

            var totalCount = models.Count;
            var filterCount = totalCount;

            if (!string.IsNullOrEmpty(filter))
            {
                models = models.Where(x => x.Title.Contains(filter)).ToList();
                filterCount = models.Count;
            }
            
            var result = models.OrderByDescending(x => x.CreateDate)
                .Select((x, i) => new 
                {
                    x.Id,
                    Number = i + 1,
                    x.Title,
                    FullName = _userService.GetFullName(x.UserId),
                    Date = x.CreateDate.ToShortPersianDateString(),
                    x.Status,
                    Time=Time.GetTimeString(x.Hour, x.Minute)
                })
                .Skip(start)
                .Take(length)
                .ToList();
            
            return Json(new {data = result, recordsTotal = totalCount, recordsFiltered = filterCount});
        }

        [HttpPost]
        public IActionResult CreateLinkForExcelFile(string userId, string dateP, string fromDate,
            string toDate, int status=-1)
        {
            var result = Url.Action("ExportExcel", "Task", new {userId, dateP, fromDate, toDate, status});
            return Json(new { link = result });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportExcel(string userId, string dateP, string fromDate,
            string toDate, int status=-1)
        {
            DateTime? filterDate=null;
            DateTime? fromDateFilter=null;
            DateTime? toDateFilter=null;
            if (!string.IsNullOrEmpty(dateP) && string.IsNullOrEmpty(toDate))
            {
                filterDate=dateP.ConvertPersianToGregorianDate();
            }
            else if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                fromDateFilter=fromDate.ConvertPersianToGregorianDate();
                toDateFilter=toDate.ConvertPersianToGregorianDate();
            }
            List <NewTask> list;
            if (userId!="-1")
            {
                if (status!=-1)
                {
                    list = await _taskService.GetAllTasks(userId, filterDate, fromDateFilter, toDateFilter, (TaskStatus)status);
                }
                else
                {
                    list = await _taskService.GetAllTasks(userId, filterDate, fromDateFilter, toDateFilter);
                }
            }
            else
            {
                if (status!=-1)
                {
                    list = await _taskService.GetAllTasks(date:filterDate, fromDate:fromDateFilter, toDate:toDateFilter, status:(TaskStatus)status);
                }
                else
                {
                    list = await _taskService.GetAllTasks(date:filterDate, fromDate:fromDateFilter, toDate:toDateFilter);
                }
            }

            var model = list.OrderByDescending(x=>x.CreateDate)
                .Select(x => new ExportExcelModel()
                {
                    FullNameUser = _userService.GetFullName(x.UserId),
                    Title = x.Title,
                    CreateDate = x.CreateDate.ToShortPersianDateTimeString(),
                    Status = x.Status == TaskStatus.Done ? "انجام شده" : "در حال انجام",
                    FinishDate = x.FinishDate.ToShortPersianDateTimeString(),
                    Time = Time.GetTimeString(x.Hour,x.Minute)
                }).ToList();
                
            var excelWriter = new ExcelWriter();
            var excelBytes = await excelWriter.GetExcelBytesAsync(model);
            return new FileContentResult(excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}