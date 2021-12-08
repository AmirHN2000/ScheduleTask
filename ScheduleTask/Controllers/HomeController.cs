using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleTask.Interface;
using ScheduleTask.Services;
using ScheduleTask.Utils;
using SmartBreadcrumbs.Attributes;

namespace ScheduleTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly UserService _userService;

        public HomeController(ITaskService taskService, UserService userService)
        {
            _taskService = taskService;
            _userService = userService;
        }

        [Authorize]
        [DefaultBreadcrumb("داشبورد")]
        public async Task<IActionResult> Dashboard(string id="")
        {
            if (User.IsInRole("Admin"))
            {
                var model1 = await _taskService.ReportTasks();
                model1.UserCount = _userService.GetUserCount();
            
                return View("DashboardAdmin", model1);
            }
            var userId = User.GetUserId();
            if (!string.IsNullOrEmpty(id))
            {
                userId = id;
            }
            var model = await _taskService.ReportTasks(userId);

            return View("Dashboard", model);
        }
    }
}