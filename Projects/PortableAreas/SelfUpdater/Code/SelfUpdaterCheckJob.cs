using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Appleseed.Framework;
using Quartz;
using SelfUpdater.Models;

namespace SelfUpdater.Code
{
    public class SelfUpdaterCheckJob : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            var projectManagers = ProjectManagerHelper.GetProjectManagers();
            var UpdateList = new List<InstallationState>();
            foreach (var projectManager in projectManagers)
            {
                var availablePackages = ProjectManagerHelper.GetAvailablePackagesLatestList(projectManager);

                var installedPackages = ProjectManagerHelper.GetInstalledPackagesLatestList(projectManager);


                foreach (var installedPackage in installedPackages)
                {
                    var update = projectManager.GetUpdatedPackage(availablePackages, installedPackage);
                    if (update != null)
                    {
                        var package = new InstallationState();
                        package.Installed = installedPackage;
                        package.Update = update;
                        package.Source = projectManager.SourceRepository.Source;

                        if (UpdateList.Any(d => d.Installed.Id == package.Installed.Id))
                        {
                            var addedPackage = UpdateList.First(d => d.Installed.Id == package.Installed.Id);
                            if (package.Update != null)
                            {
                                if (addedPackage.Update == null || addedPackage.Update.Version < package.Update.Version)
                                {
                                    UpdateList.Remove(addedPackage);
                                    UpdateList.Add(package);
                                }
                            }
                        }
                        else
                        {
                            UpdateList.Add(package);
                        }
                    }
                }
            }

            // UpdateList is a list with packages that has updates
            if(UpdateList.Any())
            {
                try
                {
                    var entities = new SelfUpdaterEntities();

                    foreach (
                        var self in
                            UpdateList.Select(
                                pack =>
                                new SelfUpdatingPackages
                                    {
                                        PackageId = pack.Installed.Id,
                                        PackageVersion = pack.Update.Version.ToString(),
                                        Source = pack.Source,
                                        Install = false
                                    }))
                    {
                        entities.AddToSelfUpdatingPackages(self);
                    }

                    entities.SaveChanges();

                    var config = WebConfigurationManager.OpenWebConfiguration("~/");
                    var section = config.GetSection("system.web/httpRuntime");
                    ((HttpRuntimeSection) section).WaitChangeNotification = 123456789;
                    ((HttpRuntimeSection) section).MaxWaitChangeNotification = 123456789;
                    config.Save();


                }
                catch(Exception e)
                {
                    ErrorHandler.Publish(LogLevel.Error, e);
                }
            }

           
        }
    }
}