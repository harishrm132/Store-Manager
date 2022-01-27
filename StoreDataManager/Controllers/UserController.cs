using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StoreDataManager.Library.DataAccess;
using StoreDataManager.Library.Models;
using StoreDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace StoreDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();
            return data.GetUserById(userId).First();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();
            using (var context = new ApplicationDbContext())
            {
                var userstore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userstore);

                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach (var usr in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = usr.Id,
                        Email = usr.Email
                    };

                    foreach (var r in usr.Roles)
                    {
                        u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).First().Name);
                    }
                    output.Add(u);
                }
            }
            return output;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);
                return roles;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/User/Admin/AddRole")]
        public void AddRole(UserRolePairModel pair)
        {
            using (var context = new ApplicationDbContext())
            {
                var userstore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userstore);

                userManager.AddToRole(pair.UserId, pair.RoleName);
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/User/Admin/RemoveRole")]
        public void RemoveRole(UserRolePairModel pair)
        {
            using (var context = new ApplicationDbContext())
            {
                var userstore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userstore);

                userManager.RemoveFromRole(pair.UserId, pair.RoleName);
            }
        }
    }
}
