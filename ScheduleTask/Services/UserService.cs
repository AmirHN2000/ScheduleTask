using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScheduleTask.Data;
using ScheduleTask.Models.User;

namespace ScheduleTask.Services
{
    public class UserService:UserManager<User>
    {
        private readonly AppDbContext _appDbContext;

        
        public UserService(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger, AppDbContext appDbContext) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer,
                errors, services, logger)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> IsExistMember(string name, string family)
        {
            var user=await _appDbContext.Users
                .FirstOrDefaultAsync(x => x.Name == name && x.Family == family);
            return user != null;
        }

        public async Task<List<User>> GetAllUser()
        {
            return (List<User>) await GetUsersInRoleAsync("User");
        }

        public async Task<ShowUserViewModel> GetUserForShow(string id)
        {
            var user = await FindByIdAsync(id);
            return user.Adapt<ShowUserViewModel>();
        }

        public string GetFullName(string userId)
        {
            var user = FindByIdAsync(userId).Result;
            return user.Name + " " + user.Family;
        }

        public int GetUserCount()
        {
            return _appDbContext
                .UserRoles
                .Count(x => x.RoleId == "a597799a-fb1b-44b2-939f-48731dec318e");
        }
    }
}