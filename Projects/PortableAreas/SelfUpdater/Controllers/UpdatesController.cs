
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
using System.Xml;
using System.Text;
using System.Dynamic;

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
            SelfUpdaterEntities context = new SelfUpdaterEntities();

            var scheduledUpdates = context.SelfUpdatingPackages.ToList();

            WebProjectManager[] projectManagers = this.GetProjectManagers();
            List<InstallationState> installed = new List<InstallationState>();
            foreach (var projectManager in projectManagers) {
                var installedPackages = this.GetInstalledPackages(projectManager);

                foreach (var installedPackage in installedPackages) {
                    IPackage update = projectManager.GetUpdate(installedPackage);
                    InstallationState package = new InstallationState();
                    package.Installed = installedPackage;
                    package.Update = update;
                    if (scheduledUpdates.Any(d => d.PackageId == installedPackage.Id)) {
                        package.Scheduled = true;
                    }


                    if (installed.Any(d => d.Installed.Id == package.Installed.Id)) {
                        var addedPackage = installed.Where(d => d.Installed.Id == package.Installed.Id).First();
                        if (package.Update != null) {
                            if (addedPackage.Update == null || addedPackage.Update.Version < package.Update.Version) {
                                installed.Remove(addedPackage);
                                installed.Add(package);
                            }
                        }
                    } else {
                        installed.Add(package);
                    }
                }
            }

            return base.View(installed);
        }

        private List<IPackage> GetInstalledPackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetInstalledPackages(string.Empty).ToList();

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

        public ActionResult Upgrade(string packageId)
        {
            try {
                WebProjectManager[] projectManagers = this.GetProjectManagers();

                WebProjectManager projectManager = null;
                IPackage installedPackage = null;
                foreach (var pm in projectManagers) {
                    projectManager = pm;
                    installedPackage = this.GetInstalledPackage(pm, packageId);
                    if (installedPackage != null) break;
                }

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


        public ActionResult DelayedUpgrade(string packageId, string source, string version)
        {
            SelfUpdaterEntities context = new SelfUpdaterEntities();

            var entity = context.SelfUpdatingPackages.Where(d => d.PackageId == packageId).FirstOrDefault();
            if (entity == default(SelfUpdatingPackages)) {

                entity = new SelfUpdatingPackages() {
                    PackageId = packageId,
                    Source = source,
                    PackageVersion = version                    
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
            SelfUpdaterEntities context = new SelfUpdaterEntities();

            var entity = context.SelfUpdatingPackages.Where(d => d.PackageId == packageId).FirstOrDefault();
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

        [AcceptVerbs(HttpVerbs.Post)]
        public void ApplyUpdates()
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
        }

        public ActionResult Status()
        {
            return Json(new {
                online = true
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