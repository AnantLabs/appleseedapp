
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
            AppleseedDBContext context = new AppleseedDBContext();

            var scheduledUpdates = context.SelfUpdatingPackages.ToList();


            WebProjectManager projectManager = this.GetProjectManager();
            var installedPackages = this.GetInstalledPackages(projectManager);
            List<InstallationState> state2 = new List<InstallationState>();
            foreach (var installedPackage in installedPackages) {
                IPackage update = projectManager.GetUpdate(installedPackage);
                InstallationState state = new InstallationState();
                state.Installed = installedPackage;
                state.Update = update;
                if (scheduledUpdates.Any(d => d.PackageId == installedPackage.Id)) {
                    state.Scheduled = true;
                }

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
            AppleseedDBContext context = new AppleseedDBContext();

            var entity = context.SelfUpdatingPackages.Where(d => d.PackageId == packageId).FirstOrDefault();
            if (entity == default(SelfUpdatingPackages)) {

                entity = new SelfUpdatingPackages() {
                    PackageId = packageId,
                    PackageVersion = string.Empty
                };

                context.SelfUpdatingPackages.AddObject(entity);
                context.SaveChanges();
            }

            return Json(new {
                msg = "Package " + packageId + " scheduled to update!",
                res = true
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RemoveDelayedUpgrade(string packageId)
        {
            AppleseedDBContext context = new AppleseedDBContext();

            var entity = context.SelfUpdatingPackages.Where(d=>d.PackageId == packageId).FirstOrDefault();
            if (entity != default(SelfUpdatingPackages)) {
                context.SelfUpdatingPackages.DeleteObject(entity);
                context.SaveChanges();


                return Json(new {
                    msg = "Package " + packageId + " unscheduled!",
                    res = true
                }, JsonRequestBehavior.AllowGet);
            } else {
                return Json(new {
                    msg = "Package " + packageId + " unscheduled!",
                    res = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ApplyUpdates()
        {
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
                msg = "Applying updates..."                
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