using NuGet;


namespace SelfUpdater.Models
{

    public class InstallationState
    {
        public IPackage Installed { get; set; }
        public IPackage Update { get; set; }
        public string Source { get; set; }
        public bool Scheduled { get; set; }
    }
}

