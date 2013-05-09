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
using Trirand.Web.Mvc;
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
        public static List<UserManagerModel> AllUsers { get; set; }

        public ActionResult Module()
        {
            var segment = Request.Url.Segments;
            var pagenumber = getPageId(segment);
            var mid = (int)ControllerContext.RouteData.Values["moduleId"];
            var urlCreate = Path.ApplicationRoot + "/DesktopModules/CoreModules/Users/UsersManage.aspx?pageId=" + pagenumber +
                            "&mID=" + mid;
            var model = new UserManagerModel {UserEmail = urlCreate};
            
            return View(model);
        }

        private string getPageId(string[] segment)
        {
            string pagenumber = 0.ToString();
            foreach (var seg in segment)
            {
                int num;
                bool isNum = int.TryParse(seg.Split('/').First(), out num);
                if (isNum)
                {
                    pagenumber = seg.Split('/').First();
                }

            }
            return pagenumber;
        } 
        public string Builddir(string email)
        {
            string userName = Membership.GetUserNameByEmail(email);
            var segment = Request.UrlReferrer.Segments;
            string pagenumber = getPageId(segment);
            string redurl = Path.ApplicationRoot + "/DesktopModules/CoreModules/Users/UsersManage.aspx?mid=" + pagenumber +
                            "&username=" + userName;
            return redurl;
        }

        public JsonResult Delete(Guid userID)
        {
            var users = new UsersDB();
            users.DeleteUser(userID);
            return Json("ok");
        }

        public JsonResult Search(string text, int page, int rows)
        {

            var data = AllUsers; 
            var result = new List<UserManagerModel>();
            var words = text.Split(' ');
            int i = 1;
            foreach (var user in data)
            {
                var name = user.UserName;
                var mail = user.UserEmail;
                var rol = user.UserRol;
                foreach (var word in words)
                {
                    var userMail = mail.Split('@');
                    if (name.ToUpper().Contains(word.ToUpper()) || (userMail[0].ToUpper().Contains(word.ToUpper())))
                    {
                        user.id = i;
                        result.Add(user);
                        i++;
                        break;
                    }
                }
                
            }
            return GetRowsFromList(result.AsQueryable(), rows, page);
        }

        public JsonResult GridUser(int page, int rows, string search, string sidx, string sord)
        {
            var data = new List<UserManagerModel>();

            var iduser = new aspnet_CustomProfile().All(orderBy: "Name").ToArray();
            var i = 1;
            foreach (var user in iduser)
            {
                var m = new UserManagerModel();
                m.id = i; 
                m.UserId = user.UserId;
                m.UserName = user.Name;
                m.UserEmail = user.Email;
                var userrolid = Guid.Parse(user.UserId.ToString());
                object[] queryargs = { userrolid };

                try
                {
                    var roleid = new aspnet_UsersInRoles().All(where: "UserId = @0", args: queryargs).Single().RoleId;
                    var rolid = Guid.Parse(roleid.ToString());
                    object[] queryargs2 = { rolid };
                    var roleName = new aspnet_Roles().All(where: "RoleId = @0", args: queryargs2).Single().RoleName;
                    m.UserRol = roleName;
                }
                catch (Exception e)
                {
                    m.UserRol = "";
                }
                m.Edit = Builddir(m.UserEmail);
                data.Add(m);
                i++;
            }

            AllUsers = data;
            var result = GetRowsFromList(data.AsQueryable(), rows, page);
            return result;
        }


        private JsonResult GetRowsFromList(IQueryable<UserManagerModel> result, int rows, int page)
        {
            var names = from m in result
                        where m.id > (rows * (page - 1))
                        select new
                        {
                            UserName = m.UserName,
                            Email = m.UserEmail,
                            Rol = m.UserRol,
                            Edit = General.GetString("EDIT_USER"),
                            UserId = m.UserId,
                            EditId = m.Edit,
                            Delete = General.GetString("DELETE_USER"),
                        };
            var totalRecords = result.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)rows);
            var jsonData = new
            {
                total = totalPages,
                page = page,
                currentPage = page,
                records = totalRecords,
                rows = names.AsQueryable()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}
