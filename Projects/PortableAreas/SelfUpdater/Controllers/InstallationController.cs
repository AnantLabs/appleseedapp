using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NuGet;
using System.Dynamic;
using System.Configuration;
using Appleseed.Framework;
using SelfUpdater.Models;

namespace SelfUpdater.Controllers
{
    public class InstallationController : BaseController
    {
        public ActionResult Module()
        {
            return View();
        }

        public ActionResult InstallModule()
        {
            try {

                var section = HttpContext.GetSection("system.web/httpRuntime") as System.Web.Configuration.HttpRuntimeSection;
                if (section.WaitChangeNotification < 5) {
                    return View("ConfigError");
                }

                var projectManagers = GetProjectManagers();
                var list = new List<dynamic>();
                var installed = projectManagers.SelectMany(d => d.GetInstalledPackages().ToList());

                foreach (var pM in projectManagers)
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

        public JsonResult InstallPackages(string packages)
        {
            try
            {
                var packagesToInstall = new JavaScriptSerializer().Deserialize<IEnumerable<PackageModel>>(packages);

                var context = new SelfUpdaterEntities();

                foreach (var self in packagesToInstall.Select(pack => new SelfUpdatingPackages { PackageId = pack.Name, PackageVersion = pack.Version, Source = pack.Source, Install = pack.Install}))
                {
                    context.AddToSelfUpdatingPackages(self);
                }

                context.SaveChanges();

                var config = WebConfigurationManager.OpenWebConfiguration("~/");
                var section = config.GetSection("system.web/httpRuntime");
                ((HttpRuntimeSection)section).WaitChangeNotification = 123456789;
                ((HttpRuntimeSection)section).MaxWaitChangeNotification = 123456789;
                config.Save();
                

                return Json("Ok");
            }
            catch(Exception e)
            {
                ErrorHandler.Publish(LogLevel.Error, e);
                Response.StatusCode = 500;
                return Json(e.Message);
            }


        }

        public ActionResult InstallPackage(string packageId, string source, string version)
        {
            //System.Web.HttpContext.Current.Session["NugetLogger"] = "Installing packages...";
            try
            {
                var projectManager = GetProjectManagers().Where(p => p.SourceRepository.Source == source).First();

                projectManager.addLog("Starting installation...");

               projectManager.InstallPackage(packageId, new SemanticVersion(version));

                projectManager.addLog("Waiting to Reload Site");

                var logger = (string) System.Web.HttpContext.Current.Application["NugetLogger"];

                return Json(new
                                {
                                    msg = "Package " + packageId + " scheduled to install!",
                                    res = true,
                                    NugetLog = logger
                                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                ErrorHandler.Publish(LogLevel.Error, e);
                Response.StatusCode = 500;
                return Json(e.Message);
            }
        }        

    }
}