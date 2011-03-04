
//using NuGet;
//using System;
//using System.Configuration;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Web.Mvc;
//using System.Collections.Generic;
//using Appleseed.Core.PackageManager;
//using Appleseed.Core.Models;

//namespace Appleseed.Core.Controllers
//{
 

//    [Authorize]
//    public class UpdatesController : Controller
//    {
//        public ActionResult Check(string packageId)
//        {
//            WebProjectManager projectManager = this.GetProjectManager();
//            var installedPackages = this.GetInstalledPackages(projectManager);
//            List<InstallationState> state2 = new List<InstallationState>();
//            foreach (var installedPackage in installedPackages)
//            {
//                IPackage update = projectManager.GetUpdate(installedPackage);
//                InstallationState state = new InstallationState();
//                state.Installed = installedPackage;
//                state.Update = update;
//                state2.Add(state);
                
                

//            }
//            if (base.Request.IsAjaxRequest())
//            {
//                var data = new
//                {
//                    Version =  string.Empty,
//                    UpdateAvailable = state2.Where(d=> d.Update != null).Count() > 0
//                };

//                return base.Json(data, JsonRequestBehavior.AllowGet);
//            }
//            else
//            {
//                return base.View(state2);
//            }
//        }

//        private List<IPackage> GetInstalledPackages(WebProjectManager projectManager)
//        {
//            var packages = projectManager.GetInstalledPackages(string.Empty).ToList();

//            //if (package == null)
//            //{
//            //    throw new InvalidOperationException(string.Format("The package for package ID '{0}' is not installed in this website. Copy the package into the App_Data/packages folder.", packageId));
//            //}
//            return packages;
//        }

//        private WebProjectManager GetProjectManager()
//        {
//            string remoteSource = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\dev\hg\AutoUpdateDemo\test-package-source";
//            return new WebProjectManager(remoteSource, base.Request.MapPath("~/"));
//        }

//        public ActionResult Upgrade(string packageId)
//        {
//            WebProjectManager projectManager = this.GetProjectManager();
//            IPackage installedPackage = this.GetInstalledPackages(projectManager).Where(d=> d.Id == packageId).FirstOrDefault();
//            IPackage update = projectManager.GetUpdate(installedPackage);
//            projectManager.UpdatePackage(update);
//            if (base.Request.IsAjaxRequest())
//            {
//                return base.Json(new { Success = true, Version = update.Version.ToString() }, JsonRequestBehavior.AllowGet);
//            }
//            return base.View(update);
//        }
//    }
//}

