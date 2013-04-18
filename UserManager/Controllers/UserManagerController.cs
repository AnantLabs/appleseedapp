using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserManager.Massive;
using UserManager.Models;
using aspnet_CustomProfile = UserManager.Massive.aspnet_CustomProfile;

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
            var model = new UserManagerModel();

            object[] queryargs2 = { 1 };

            var iduser = new aspnet_CustomProfile().All().ToArray();
            //model.UserId = iduser[0].UserID;
            model.UserName = iduser[0].Name;
            model.UserEmail = iduser[0].Email;

            var result = new aspnet_CustomProfile().Paged();

            return View(model);
        }
    }
}
