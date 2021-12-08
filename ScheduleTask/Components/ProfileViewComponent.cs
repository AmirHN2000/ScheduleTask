using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using ScheduleTask.Models;
using ScheduleTask.Models.Components;
using ScheduleTask.Services;
using ScheduleTask.Utils;

namespace ScheduleTask.Components
{
    public class ProfileViewComponent:ViewComponent
    {
        private UserService _userService;

        public ProfileViewComponent(UserService userService)
        {
            _userService = userService;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserForShow(UserClaimsPrincipal.GetUserId());
            var model = new ProfileViewModel()
            {
                Id = user.Id,
                FuulName = user.Name + " " + user.Family
            };
            return View("_default", model);
        }
    }
}