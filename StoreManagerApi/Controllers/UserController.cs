using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StoreDataManager.Library.DataAccess;
using StoreDataManager.Library.Models;
using StoreManagerApi.Data;
using StoreManagerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreManagerApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserData userData;
        private readonly ILogger logger;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            IUserData userData, ILogger logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.userData = userData;
            this.logger = logger;
        }

        [HttpGet]
        public UserModel GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userData.GetUserById(userId).First();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var users = context.Users.ToList();
            //OuterSeq.Join(InnerSeq, OuterKey, InnerKey, Select Statement)
            var userRoles = context.UserRoles.Join(context.Roles, ur => ur.RoleId, r => r.Id, 
                (ur, r) => new
                {
                    ur.UserId, ur.RoleId, r.Name
                });

            foreach (var usr in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = usr.Id,
                    Email = usr.Email
                };

                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);
                output.Add(u);
            }
            
            return output;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/AddRole")]
        public async Task AddRoleAsync(UserRolePairModel pair)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var iUser = await userManager.FindByIdAsync(pair.UserId);
            logger.LogInformation("Admin {Admin} to add user {User} to role {Role}", loggedInUserId, iUser.Id, pair.RoleName);

            await userManager.AddToRoleAsync(iUser, pair.RoleName);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/RemoveRole")]
        public async Task RemoveRoleAsync(UserRolePairModel pair)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var iUser = await userManager.FindByIdAsync(pair.UserId);
            logger.LogInformation("Admin {Admin} to add user {User} to role {Role}", loggedInUserId, iUser.Id, pair.RoleName);

            await userManager.RemoveFromRoleAsync(iUser, pair.RoleName);
        }
    }
}
