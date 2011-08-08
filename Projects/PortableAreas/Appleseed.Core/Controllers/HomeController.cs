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
        public JsonResult TestValue()
        {
            return Json("testString");
        }

        public ActionResult SaveTitle(string id, string value){
            try {
                var parse = id.Split('_');
                int moduleId = Int32.Parse(parse[1]);
                // Si el usuario tiene permiso para modificar
                if (PortalSecurity.HasEditPermissions(moduleId)) {
                    
                    var modules = new Appleseed.Framework.Site.Data.ModulesDB();

                    modules.UpdateModuleTitle(moduleId, value);                    
                    
                    return Json(new { result = true });

                }
                return Json(new { result = false });
            } catch (Exception) {
                return Json(new { result = false });
            }
        }
    }
}
