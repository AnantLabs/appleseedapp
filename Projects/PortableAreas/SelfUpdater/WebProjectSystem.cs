namespace System.Web.WebPages.Administration.PackageManager
{
    using NuGet;
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;

    public class WebProjectSystem : PhysicalFileSystem, IProjectSystem, IFileSystem
    {
        private const string BinDir = "bin";

        public WebProjectSystem(string root) : base(root)
        {
        }

        public void AddReference(string referencePath, Stream stream)
        {
            string fileName = Path.GetFileName(referencePath);
            string fullPath = this.GetFullPath(this.GetReferencePath(fileName));
            this.AddFile(fullPath, stream);
        }

        
        public dynamic GetPropertyValue(string propertyName)
        {
            if ((propertyName != null) && propertyName.Equals("RootNamespace", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }
            return null;
        }

        protected virtual string GetReferencePath(string name)
        {
            return Path.Combine("bin", name);
        }

        public bool IsSupportedFile(string path)
        {
            return (!path.StartsWith("tools", StringComparison.OrdinalIgnoreCase) && !Path.GetFileName(path).Equals("app.config", StringComparison.OrdinalIgnoreCase));
        }

        public bool ReferenceExists(string name)
        {
            string referencePath = this.GetReferencePath(name);
            return this.FileExists(referencePath);
        }

        public void RemoveReference(string name)
        {
            this.DeleteFile(this.GetReferencePath(name));
            if (!this.GetFiles("bin").Any<string>())
            {
                this.DeleteDirectory("bin");
            }
        }

        public string ProjectName
        {
            get
            {
                return base.Root;
            }
        }

        public FrameworkName TargetFramework
        {
            get
            {
                return VersionUtility.DefaultTargetFramework;
            }
        }

        public void AddFrameworkReference(string name)
        {
            throw new NotImplementedException();
        }

        public string ResolvePath(string path)
        {
            return path;
        }
    }
}

