using NuGet;
using System;
using System.Runtime.CompilerServices;


namespace SelfUpdater.Models
{

    public class InstallationState
    {
        public IPackage Installed { get; set; }
        public IPackage Update { get; set; }

        public bool Scheduled { get; set; }
    }
}

