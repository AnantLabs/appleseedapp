using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Appleseed.Framework;
using Microsoft.AspNet.SignalR;
using NuGet;
using SelfUpdater.Controllers;
using SelfUpdater.Models;

namespace Appleseed.Code
{
    public class SelfUpdateThread
    {

        public void CheckForSelfUpdates()
        {
            try
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<SelfUpdater.SignalR.SelfUpdaterHub>();

                var context = new SelfUpdaterEntities();

                var packagesToUpdate = context.SelfUpdatingPackages.AsQueryable();

                //updateNeeded = (packagesToUpdate.Count() > 0);

                if (packagesToUpdate.Any())
                {
                    var packagesToInstall = packagesToUpdate.Where(packages => packages.Install == true);

                    foreach (var package in packagesToInstall)
                    {
                        try
                        {
                            var projectManager =
                                GetProjectManagers().Where(p => p.SourceRepository.Source == package.Source).First();


                            hub.Clients.All.openPopUp();

                            projectManager.addLog("Installing " + package.PackageId);

                            ErrorHandler.Publish(LogLevel.Info,
                                                 String.Format("SelfUpdater: Installing {0} to version {1}",
                                                               package.PackageId, package.PackageVersion));

                            projectManager.InstallPackage(package.PackageId, new SemanticVersion(package.PackageVersion));

                            context.SelfUpdatingPackages.DeleteObject(package);
                        }
                        catch(Exception e)
                        {
                            ErrorHandler.Publish(LogLevel.Error, e);
                        }
                    }

                    var packagesToUpgrade = packagesToUpdate.Where(packages => packages.Install == false);
                    foreach (var package in packagesToUpgrade)
                    {
                        try
                        {


                            var projectManager =
                                GetProjectManagers().Where(p => p.SourceRepository.Source == package.Source).First();

                            projectManager.addLog("Updating " + package.PackageId);

                            IPackage installedPackage = GetInstalledPackage(projectManager, package.PackageId);

                            IPackage update = projectManager.GetUpdatedPackage(installedPackage);

                            projectManager.UpdatePackage(update);

                            context.SelfUpdatingPackages.DeleteObject(package);
                        }
                        catch(Exception e)
                        {
                            ErrorHandler.Publish(LogLevel.Error, e);
                        }
                    }






                    context.SaveChanges();

                    var config = WebConfigurationManager.OpenWebConfiguration("~/");
                    var section = config.GetSection("system.web/httpRuntime");
                    ((HttpRuntimeSection)section).WaitChangeNotification = 10;
                    ((HttpRuntimeSection)section).MaxWaitChangeNotification = 10;
                    config.Save();

                }
                else
                {
                    ErrorHandler.Publish(LogLevel.Info, "SelfUpdater: Nothing to update");
                }


                hub.Clients.All.reloadPage();
                
            }
            catch (Exception exc)
            {

                ErrorHandler.Publish(LogLevel.Error, exc);
                
            }
        }


        private WebProjectManager[] GetProjectManagers()
        {
            var remoteSources = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\";
            var managers = new List<WebProjectManager>();
            foreach (var remoteSource in remoteSources.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                managers.Add(new WebProjectManager(remoteSource, HttpRuntime.AppDomainAppPath));
            }

            return managers.ToArray();
        }

        private IPackage GetInstalledPackage(WebProjectManager projectManager, string packageId)
        {
            IPackage package = projectManager.GetInstalledPackages().Where(d => d.Id == packageId).FirstOrDefault();

            if (package == null)
            {
                throw new InvalidOperationException(string.Format("The package for package ID '{0}' is not installed in this website. Copy the package into the App_Data/packages folder.", packageId));
            }
            return package;
        }

    }
}