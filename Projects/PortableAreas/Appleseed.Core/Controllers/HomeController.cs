using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using Appleseed.Framework.Security;
using Appleseed.Framework.Site.Configuration;


namespace Appleseed.Core.Controllers
{
   
    public class HomeController :Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ListResources()
        {
            string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            ViewData.Model = resources;
            return View();
        }

        public JsonResult TestValue()
        {
            return Json("testString");
        }

        public void SaveTitle(string id, string value){
            var parse = id.Split('_');
            int moduleId = Int32.Parse(parse[1]);
            // Si el usuario tiene permiso para modificar
            if (PortalSecurity.HasEditPermissions(moduleId)) {
                PortalSettings settings = (PortalSettings)HttpContext.Items["PortalSettings"];
                var m = settings.ActivePage.Modules.Cast<ModuleSettings>().FirstOrDefault(mod => mod.ModuleID == moduleId);

                if (m == null) {
                    return;
                }

                var modules = new Appleseed.Framework.Site.Data.ModulesDB();
                modules.UpdateModule(
                m.PageID,
                m.ModuleID,
                m.ModuleOrder,
                m.PaneName,
                value,
                m.CacheTime,
                m.AuthorizedEditRoles,
                m.AuthorizedViewRoles,
                m.AuthorizedAddRoles,
                m.AuthorizedDeleteRoles,
                m.AuthorizedPropertiesRoles,
                m.AuthorizedMoveModuleRoles,
                m.AuthorizedDeleteModuleRoles,
                m.ShowMobile,
                m.AuthorizedPublishingRoles,
                m.SupportWorkflow,
                m.AuthorizedApproveRoles,
                m.ShowEveryWhere,
                m.SupportCollapsable);



            }
        }
    }
}
