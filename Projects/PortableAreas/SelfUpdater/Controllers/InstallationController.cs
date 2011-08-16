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
            var projectManagers = GetProjectManagers();
            var list = new List<dynamic>();

            foreach (var pM in projectManagers) {
                var packages = GetAvailablePackages(pM);
                foreach (var package in packages) {
                    dynamic p = new ExpandoObject();
                    p.icon = package.IconUrl;
                    p.name = package.Id;
                    p.version = package.Version;

                    list.Add(p);
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
    }
}