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
    public class BaseController : Controller
    {

        protected WebProjectManager[] GetProjectManagers()
        {
            string remoteSources = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\";
            List<WebProjectManager> managers = new List<WebProjectManager>();
            foreach (var remoteSource in remoteSources.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)) {
                managers.Add(new WebProjectManager(remoteSource, base.Request.MapPath("~/")));
            }

            return managers.ToArray();
        }

        protected List<IPackage> GetInstalledPackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetInstalledPackages(string.Empty).ToList();

            return packages;
        }

        protected List<IPackage> GetAvailablePackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetRemotePackages(string.Empty).ToList();

            return packages;
        }

    }
}
