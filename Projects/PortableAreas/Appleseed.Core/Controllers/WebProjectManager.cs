//using NuGet;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Web.WebPages.Administration.PackageManager;


//namespace Appleseed.Core.PackageManager
//{
  

//    internal class WebProjectManager
//    {
//        private readonly IProjectManager _projectManager;

//        public WebProjectManager(string remoteSource, string siteRoot)
//        {
//            string webRepositoryDirectory = GetWebRepositoryDirectory(siteRoot);
//            IPackageRepository repository = PackageRepositoryFactory.Default.CreateRepository((PackageSource) remoteSource);
//            IPackagePathResolver resolver = new DefaultPackagePathResolver(webRepositoryDirectory);
//            IPackageRepository repository2 = PackageRepositoryFactory.Default.CreateRepository((PackageSource) webRepositoryDirectory);
//            IProjectSystem system = new WebProjectSystem(siteRoot);
//            this._projectManager = new ProjectManager(repository, resolver, system, repository2);
//        }

//        public IQueryable<IPackage> GetInstalledPackages(string searchTerms)
//        {
//            return GetPackages(this.LocalRepository, searchTerms);
//        }

//        private static IEnumerable<IPackage> GetPackageDependencies(IPackage package, IPackageRepository localRepository, IPackageRepository sourceRepository)
//        {
//            IPackageRepository repository = localRepository;
//            IPackageRepository repository2 = sourceRepository;
//            ILogger instance = NullLogger.Instance;
//            bool ignoreDependencies = false;
//            InstallWalker walker = new InstallWalker(repository, repository2, instance, ignoreDependencies);
//            return walker.ResolveOperations(package).Where<PackageOperation>(delegate (PackageOperation operation) {
//                return (operation.Action == PackageAction.Install);
//            }).Select<PackageOperation, IPackage>(delegate (PackageOperation operation) {
//                return operation.Package;
//            });
//        }

//        internal static IQueryable<IPackage> GetPackages(IQueryable<IPackage> packages, string searchTerm)
//        {
//            if (!string.IsNullOrEmpty(searchTerm))
//            {
//                searchTerm = searchTerm.Trim();
//                packages = packages.Find(searchTerm.Split(new char[0]));
//            }
//            return packages;
//        }

//        internal static IQueryable<IPackage> GetPackages(IPackageRepository repository, string searchTerm)
//        {
//            return GetPackages(repository.GetPackages(), searchTerm);
//        }

//        internal IEnumerable<IPackage> GetPackagesRequiringLicenseAcceptance(IPackage package)
//        {
//            IPackageRepository localRepository = this.LocalRepository;
//            IPackageRepository sourceRepository = this.SourceRepository;
//            return GetPackagesRequiringLicenseAcceptance(package, localRepository, sourceRepository);
//        }

//        internal static IEnumerable<IPackage> GetPackagesRequiringLicenseAcceptance(IPackage package, IPackageRepository localRepository, IPackageRepository sourceRepository)
//        {
//            return GetPackageDependencies(package, localRepository, sourceRepository).Where<IPackage>(delegate (IPackage p) {
//                return p.RequireLicenseAcceptance;
//            });
//        }

//        public IQueryable<IPackage> GetPackagesWithUpdates(string searchTerms)
//        {
//            return GetPackages(PackageRepositoryExtensions.GetUpdates(this.LocalRepository, this.SourceRepository.GetPackages()).AsQueryable<IPackage>(), searchTerms);
//        }

//        public IQueryable<IPackage> GetRemotePackages(string searchTerms)
//        {
//            return GetPackages(this.SourceRepository, searchTerms);
//        }

//        public IPackage GetUpdate(IPackage package)
//        {
//            return PackageRepositoryExtensions.GetUpdates(this.SourceRepository, this.LocalRepository.GetPackages()).FirstOrDefault<IPackage>(delegate (IPackage p) {
//                return (package.Id == p.Id);
//            });
//        }

//        internal static string GetWebRepositoryDirectory(string siteRoot)
//        {
//            return Path.Combine(siteRoot, "App_Data", "packages");
//        }

//        public IEnumerable<string> InstallPackage(IPackage package)
//        {
//            return this.PerformLoggedAction(delegate {
//                bool ignoreDependencies = false;
//                this._projectManager.AddPackageReference(package.Id, package.Version, ignoreDependencies);
//            });
//        }

//        public bool IsPackageInstalled(IPackage package)
//        {
//            return this.LocalRepository.Exists(package);
//        }

//        private IEnumerable<string> PerformLoggedAction(Action action)
//        {
//            ErrorLogger logger = new ErrorLogger();
//            this._projectManager.Logger = logger;
//            try
//            {
//                action();
//            }
//            finally
//            {
//                this._projectManager.Logger = null;
//            }
//            return logger.Errors;
//        }

//        public IEnumerable<string> UninstallPackage(IPackage package, bool removeDependencies)
//        {
//            return this.PerformLoggedAction(delegate {
//                bool forceRemove = false;
//                bool flag1 = removeDependencies;
//                this._projectManager.RemovePackageReference(package.Id, forceRemove, flag1);
//            });
//        }

//        public IEnumerable<string> UpdatePackage(IPackage package)
//        {
//            return this.PerformLoggedAction(delegate {
//                bool updateDependencies = true;
//                this._projectManager.UpdatePackageReference(package.Id, package.Version, updateDependencies);
//            });
//        }

//        public IPackageRepository LocalRepository
//        {
//            get
//            {
//                return this._projectManager.LocalRepository;
//            }
//        }

//        public IPackageRepository SourceRepository
//        {
//            get
//            {
//                return this._projectManager.SourceRepository;
//            }
//        }

//        private class ErrorLogger : ILogger
//        {
//            private readonly IList<string> _errors = new List<string>();

//            public void Log(MessageLevel level, string message, params object[] args)
//            {
//                if (level == MessageLevel.Warning)
//                {
//                    this._errors.Add(string.Format(CultureInfo.CurrentCulture, message, args));
//                }
//            }

//            public IEnumerable<string> Errors
//            {
//                get
//                {
//                    return this._errors;
//                }
//            }
//        }
//    }
//}

