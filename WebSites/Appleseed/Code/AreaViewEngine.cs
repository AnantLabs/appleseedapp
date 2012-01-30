using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using StructureMap;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Appleseed.Code
{
    public class AreaViewEngine : WebFormViewEngine
    {
        public AreaViewEngine()
            : base()
        {
            var ViewLocationFormatsList = new List<string>
            {               
                "~/{0}.aspx",
                "~/{0}.ascx",
                "~/{0}.cshtml",
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx",
                "~/Views/Shared/{0}.cshtml",

            };

            var MasterLocationFormatsList = new List<string>
            {
                "~/{0}.master",
                "~/Shared/{0}.master",
                "~/Views/{1}/{0}.master",
                "~/Views/Shared/{0}.master",
            };


            ViewLocationFormatsList.Insert(0, @"~/Portals/_$$/MVCTemplates/{0}.ascx");
            ViewLocationFormatsList.Insert(1, @"~/Portals/_$$/MVCTemplates/{0}.cshtml");
            ViewLocationFormats = ViewLocationFormatsList.ToArray();
            PartialViewLocationFormats = ViewLocationFormats;

            MasterLocationFormats = MasterLocationFormatsList.ToArray();
            AreaMasterLocationFormats = MasterLocationFormats;
        }

        private string PortalAlias
        {
            get
            {
                return (string)HttpContext.Current.Items["PortalAlias"];
            }
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            ViewLocationFormats[0] = ViewLocationFormats[0].Replace("$$", PortalAlias);
            PartialViewLocationFormats[0] = PartialViewLocationFormats[0].Replace("$$", PortalAlias);
            ViewLocationFormats[1] = ViewLocationFormats[1].Replace("$$", PortalAlias);
            PartialViewLocationFormats[1] = PartialViewLocationFormats[1].Replace("$$", PortalAlias);

            ViewEngineResult result = null;

            /*custom partialview exists ?*/
            var formattedView = FormatViewName(controllerContext, partialViewName);
            string str2 = formattedView.path;
            if (formattedView.custom) {
                
                if(str2.EndsWith(".cshtml"))
                    return new ViewEngineResult(new RazorView(controllerContext,str2,null,false,null),this);
                else
                    return new ViewEngineResult(new WebFormView(controllerContext,str2), this);
            }

            result = base.FindPartialView(controllerContext, str2, useCache);
            if ((result != null) && (result.View != null)) {
                return result;
            }
            /**/

            /*or custom shared partialview exists ?*/
            formattedView = FormatSharedViewName(controllerContext, partialViewName);
            string str3 = formattedView.path;
            if (formattedView.custom) {
                if (str3.EndsWith(".cshtml"))
                    return new ViewEngineResult(new RazorView(controllerContext, str3, null, false, null), this);
                else
                    new ViewEngineResult(new WebFormView(controllerContext, str3), this);
            }

            result = base.FindPartialView(controllerContext, str3, useCache);
            if ((result != null) && (result.View != null)) {
                return result;
            }

            /*else return original partialview*/


            var originalviewMVC2 = base.FindPartialView(controllerContext, partialViewName, useCache);

            if (originalviewMVC2.View != null) {

                return originalviewMVC2;
            }
            else {
                return new RazorViewEngine().FindPartialView(controllerContext, partialViewName, useCache);
            }

        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewLocationFormats[0] = ViewLocationFormats[0].Replace("$$", PortalAlias);
            PartialViewLocationFormats[0] = PartialViewLocationFormats[0].Replace("$$", PortalAlias);
            ViewLocationFormats[1] = ViewLocationFormats[1].Replace("$$", PortalAlias);
            PartialViewLocationFormats[1] = PartialViewLocationFormats[1].Replace("$$", PortalAlias);

            ViewEngineResult result = null;

            /*custom partialview exists ?*/
            var formattedView = FormatViewName(controllerContext, viewName);
            string str2 = formattedView.path;
            if (formattedView.custom) {

                if (str2.EndsWith(".cshtml"))
                    return new ViewEngineResult(new RazorView(controllerContext, str2, null, false, null), this);
                else
                    return new ViewEngineResult(new WebFormView(controllerContext, str2), this);
            }

            result = base.FindPartialView(controllerContext, str2, useCache);
            if ((result != null) && (result.View != null)) {
                return result;
            }
            /**/

            /*or custom shared partialview exists ?*/
            formattedView = FormatSharedViewName(controllerContext, viewName);
            string str3 = formattedView.path;
            if (formattedView.custom) {

                if (str3.EndsWith(".cshtml"))
                    return new ViewEngineResult(new RazorView(controllerContext, str3, null, false, null), this);
                else
                    new ViewEngineResult(new WebFormView(controllerContext, str3), this);

                 
            }

            result = base.FindPartialView(controllerContext, str3, useCache);
            if ((result != null) && (result.View != null)) {
                return result;
            }

            /*else return original partialview*/
            var originalviewMVC2 = base.FindView(controllerContext, viewName, masterName, useCache);

            if (originalviewMVC2.View != null) {

                return originalviewMVC2;
            }
            else {
                return new RazorViewEngine().FindView(controllerContext, viewName, masterName, useCache);
            }
        }

        private dynamic FormatViewName(ControllerContext controllerContext, string viewName)
        {
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string str2 = Convert.ToString(controllerContext.RouteData.Values["area"] ?? string.Empty);

            var basePath = "Areas";

            var tempPath = @"~/Portals/_" + PortalAlias + "/MVCTemplates/";
            var relativeFilePath = Path.WebPathCombine(tempPath, str2, "Views", requiredString, viewName);
            var absoluteFilePath = controllerContext.HttpContext.Server.MapPath(relativeFilePath);

            if (FileExists(controllerContext, Path.WebPathCombine(tempPath, str2, "Views", requiredString, viewName))) {
                basePath = tempPath;
            } else {
                if (System.IO.File.Exists(absoluteFilePath + ".ascx")) {
                    return new { path = relativeFilePath + ".ascx", custom = true };
                }
                if (System.IO.File.Exists(absoluteFilePath + ".aspx")) {
                    return new { path = relativeFilePath + ".aspx", custom = true };
                }
                if (System.IO.File.Exists(absoluteFilePath + ".cshtml")) {
                    return new { path = relativeFilePath + ".cshtml", custom = true };
                }
            }

            return new { path = Path.WebPathCombine(basePath, str2, "Views", requiredString, viewName), custom = false };
        }

        private dynamic FormatSharedViewName(ControllerContext controllerContext, string viewName)
        {
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string str = Convert.ToString(controllerContext.RouteData.Values["area"] ?? string.Empty);

            var basePath = "Areas";

            var tempPath = @"~/Portals/_" + PortalAlias + "/MVCTemplates/";
            var relativeFilePath = Path.WebPathCombine(tempPath, str, "Views", "Shared", viewName);
            var absoluteFilePath = controllerContext.HttpContext.Server.MapPath(relativeFilePath);

            if (FileExists(controllerContext, Path.WebPathCombine(tempPath, str, "Views", "Shared", viewName))) {
                basePath = tempPath;
            } else {
                if (System.IO.File.Exists(absoluteFilePath + ".ascx")) {
                    return new { path = relativeFilePath + ".ascx", custom = true };
                }
                if (System.IO.File.Exists(absoluteFilePath + ".aspx")) {
                    return new { path = relativeFilePath + ".aspx", custom = true };
                }
                if (System.IO.File.Exists(absoluteFilePath + ".cshtml")) {
                    return new { path = relativeFilePath + ".cshtml", custom = true };
                }
            }


            return new { path = Path.WebPathCombine(basePath, str, "Views", "Shared", viewName), custom = false };
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string newMasterPath = masterPath;

            if (viewPath.ToLower().EndsWith(".aspx")) {
                string customMasterPath = "~/Portals/_" + PortalAlias + "/MVCTemplates/Menu.master";
                if (System.IO.File.Exists(controllerContext.HttpContext.Server.MapPath(customMasterPath))) {
                    newMasterPath = customMasterPath;
                }
            }

            var view = base.CreateView(controllerContext, viewPath, newMasterPath);

            return view;
        }
    }
}