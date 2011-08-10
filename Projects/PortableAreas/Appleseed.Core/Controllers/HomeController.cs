using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using Appleseed.Framework.Security;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Core;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.Framework.Settings;
using System.Resources;


namespace Appleseed.Core.Controllers
{
   
    public class HomeController :Controller
    {
        public JsonResult TestValue()
        {
            return Json("testString");
        }

        public ActionResult LstLanguages()
        {
            LanguageSwitcher ls = new LanguageSwitcher();
            Appleseed.Framework.Web.UI.WebControls.LanguageCultureCollection lcc = Appleseed.Framework.Localization.LanguageSwitcher.GetLanguageCultureList();


            List<string[]> datos = new List<string[]>();
            
            foreach (LanguageCultureItem l in lcc)
            {
                string[] dato = new string[3];
                dato[0] = l.ToString();
                LanguageSwitcher lswitcher = new LanguageSwitcher();
                dato[1] = lswitcher.GetFlagImgLCI(l);
                String path = Request.ApplicationPath;

                dato[1] = dato[1].Replace("images", path+"aspnet_client");
                dato[2] = lswitcher.getNameLCI(l);
                datos.Add(dato);
            }

            ViewData["datos"] = datos;
            return View();
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
