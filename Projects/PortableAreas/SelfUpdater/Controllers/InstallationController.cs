using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGet;
using System.Dynamic;
using System.Configuration;

namespace SelfUpdater.Controllers
{
    public class InstallationController : Controller
    {
        public ActionResult Module()
        {
            var section = HttpContext.GetSection("system.web/httpRuntime") as System.Web.Configuration.HttpRuntimeSection;
            if (section.WaitChangeNotification < 5) {
                return View("ConfigError");
            }

            var projectManagers = GetProjectManagers();
            var list = new List<dynamic>();
            var installed = projectManagers.SelectMany(d=>d.GetInstalledPackages(string.Empty).ToList());

            foreach (var pM in projectManagers) {
                var packages = GetAvailablePackages(pM);
                foreach (var package in packages) {
                    if (!installed.Any(d=> d.Id == package.Id)) {
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

        private List<IPackage> GetAvailablePackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetRemotePackages(string.Empty).ToList();

            return packages;
        }

        private WebProjectManager[] GetProjectManagers()
        {
            string remoteSources = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\";
            List<WebProjectManager> managers = new List<WebProjectManager>();
            foreach (var remoteSource in remoteSources.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)) {
                managers.Add(new WebProjectManager(remoteSource, base.Request.MapPath("~/")));
            }

            return managers.ToArray();
        }

        public ActionResult InstallPackage(string packageId, string source)
        {

            var projectManager = GetProjectManagers().Where(p => p.SourceRepository.Source == source).First();

            projectManager.InstallPackage(projectManager.GetRemotePackages(string.Empty).Where(d => d.Id == packageId).First());

            return Json(new {
                msg = "Package " + packageId + " scheduled to install!",
                res = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}