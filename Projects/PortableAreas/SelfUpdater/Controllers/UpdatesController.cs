
using NuGet;
using System;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Collections.Generic;
using SelfUpdater.Models;
using Appleseed.Framework;
using System.IO;
using Appleseed.Core.Models;
using System.Xml;
using System.Text;

namespace SelfUpdater.Controllers
{
    [Authorize]
    public class UpdatesController : Controller
    {
        public UpdatesController()
        {
        }

        public ActionResult Module()
        {
            //    return View();
            //}

            //public ActionResult Check(string packageId)
            //{
            WebProjectManager projectManager = this.GetProjectManager();
            var installedPackages = this.GetInstalledPackages(projectManager);
            List<InstallationState> state2 = new List<InstallationState>();
            foreach (var installedPackage in installedPackages) {
                IPackage update = projectManager.GetUpdate(installedPackage);
                InstallationState state = new InstallationState();
                state.Installed = installedPackage;
                state.Update = update;
                state2.Add(state);
            }

            if (base.Request.IsAjaxRequest()) {
                var data = new {
                    Version = string.Empty,
                    UpdateAvailable = state2.Where(d => d.Update != null).Count() > 0
                };

                return base.Json(data, JsonRequestBehavior.AllowGet);
            } else {
                return base.View(state2);
            }
        }

        private List<IPackage> GetInstalledPackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetInstalledPackages(string.Empty).ToList();

            //if (package == null)
            //{
            //    throw new InvalidOperationException(string.Format("The package for package ID '{0}' is not installed in this website. Copy the package into the App_Data/packages folder.", packageId));
            //}

            return packages;
        }

        private WebProjectManager GetProjectManager()
        {
            string remoteSource = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\";
            return new WebProjectManager(remoteSource, base.Request.MapPath("~/"));
        }

        public ActionResult Upgrade(string packageId)
        {
            try {
                WebProjectManager projectManager = this.GetProjectManager();
                IPackage installedPackage = this.GetInstalledPackage(projectManager, packageId);

                IPackage update = projectManager.GetUpdate(installedPackage);

                projectManager.UpdatePackage(update);


                return Json(new {
                    msg = "Updated " + packageId + " to " + update.Version.ToString() + "!",
                    updated = true
                }, JsonRequestBehavior.AllowGet);
            } catch (Exception exc) {
                ErrorHandler.Publish(LogLevel.Error, exc);

                return Json(new {
                    msg = "Error updating " + packageId,
                    updated = false
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DelayedUpgrade(string packageId)
        {
            //string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", packageId + ".update");

            //System.IO.File.Create(basePath);

            AppleseedDBContext context = new AppleseedDBContext();

            var entity = new SelfUpdatingPackages() {
                PackageId = packageId,
                PackageVersion = string.Empty
            };

            context.SelfUpdatingPackages.AddObject(entity);
            context.SaveChanges();

            /*Forcing site restart*/
            var doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            var configFile = Server.MapPath("~/web.config");
            doc.Load(configFile);

            var writer = new XmlTextWriter(configFile, Encoding.UTF8) { Formatting = Formatting.Indented };
            doc.Save(writer);
            writer.Flush();
            writer.Close();
            /*....................*/

            return Json(new {
                msg = "Package " + packageId + " scheduled to update!",
                updated = true
            }, JsonRequestBehavior.AllowGet);
        }

        private IPackage GetInstalledPackage(WebProjectManager projectManager, string packageId)
        {
            IPackage package = projectManager.GetInstalledPackages(string.Empty).Where(d => d.Id == packageId).FirstOrDefault();

            if (package == null) {
                throw new InvalidOperationException(string.Format("The package for package ID '{0}' is not installed in this website. Copy the package into the App_Data/packages folder.", packageId));
            }
            return package;
        }
    }
}