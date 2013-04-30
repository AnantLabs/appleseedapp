using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Appleseed.Framework;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Users.Data;
using UserManager.Massive;
using UserManager.Models;
using aspnet_CustomProfile = UserManager.Massive.aspnet_CustomProfile;
using Appleseed.Framework.Web.UI.WebControls;

namespace UserManager.Controllers
{
    public class UserManagerController : Controller
    {
        //
        // GET: /UserManager/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Module()
        {

            var model = new List<UserManagerModel>();

            var iduser = new aspnet_CustomProfile().All(orderBy: "Name").ToArray();
            foreach (var user in iduser)
            {
                var m = new UserManagerModel();
                m.UserId = user.UserId;
                m.UserName = user.Name;
                m.UserEmail = user.Email;
                var userrolid = Guid.Parse(user.UserId.ToString());
                object[] queryargs = { userrolid };
               
                try
                {
                    var roleid = new aspnet_UsersInRoles().All(where: "UserId = @0", args: queryargs).Single().RoleId;
                    var rolid = Guid.Parse(roleid.ToString());
                    object[] queryargs2 = {rolid};
                    var roleName = new aspnet_Roles().All(where: "RoleId = @0", args: queryargs2).Single().RoleName;
                    m.UserRol = roleName;
                }
                catch (Exception e)
                {
                    m.UserRol = "";
                }
                m.Edit = Builddir(m.UserEmail);
                model.Add(m);
            }

            var result = new aspnet_CustomProfile().Paged();

            return View(model);
        }

        public string Builddir(string email)
        {
            string userName = Membership.GetUserNameByEmail(email);
            string pagenumber = Request.Url.Segments[2].Split('/').First();
            string redurl = Path.ApplicationRoot + "/DesktopModules/CoreModules/Users/UsersManage.aspx?mid=" + 281 +
                            "&username=" + userName;
            return redurl;
        }

        public JsonResult Delete(Guid userID)
        {
            //Guid userId = Guid.Parse(userID);
            var users = new UsersDB();
            users.DeleteUser(userID);
            return Json("ok");
        }
        
    }
}
