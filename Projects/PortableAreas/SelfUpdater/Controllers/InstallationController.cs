using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGet;
using System.Dynamic;
using System.Configuration;
using Appleseed.Framework;

namespace SelfUpdater.Controllers
{
    public class InstallationController : BaseController
    {
        public ActionResult Module()
        {
            return View();
        }

        public ActionResult InstallModule() {
            try {
                var section = HttpContext.GetSection("system.web/httpRuntime") as System.Web.Configuration.HttpRuntimeSection;
                if (section.WaitChangeNotification < 5) {
                    return View("ConfigError");
                }

                var projectManagers = GetProjectManagers();
                var list = new List<dynamic>();
                var installed = projectManagers.SelectMany(d => d.GetInstalledPackages(string.Empty).ToList());

                foreach (var pM in projectManagers.Where(x => x.SourceRepository.Source != "https://nuget.org/api/v2/"))
                {
                    var packages = GetAvailablePackages(pM);
                    foreach (var package in packages) {
                        if (!installed.Any(d => d.Id == package.Id)) {
                            dynamic p = new ExpandoObject();
                            p.icon = package.IconUrl;
                            p.icon = p.icon ?? string.Empty;
                            p.name = package.Id;
                            p.version = package.Version;
                            p.author = package.Authors.FirstOrDefault();
                            p.source = pM.SourceRepository.Source;

                            list.Add(p);
                        }
                    }
                }

                return View(list);
            }
            catch (Exception e) {
                ErrorHandler.Publish(LogLevel.Error, "Nuget Get packages from feed", e);
                return View("ExceptionError");
            }
        
        }

        public ActionResult InstallPackage(string packageId, string source)
        {
            //System.Web.HttpContext.Current.Session["NugetLogger"] = "Installing packages...";
            
            var projectManager = GetProjectManagers().Where(p => p.SourceRepository.Source == source).First();

            projectManager.addLog("Starting installation...");            

            projectManager.InstallPackage(projectManager.GetRemotePackages(string.Empty).Where(d => d.Id == packageId).First());

            projectManager.addLog("Waiting to Reload Site");

            var logger = (string)System.Web.HttpContext.Current.Application["NugetLogger"];

            return Json(new {
                msg = "Package " + packageId + " scheduled to install!",
                res = true,
                NugetLog = logger
            }, JsonRequestBehavior.AllowGet);
        }        

    }
}