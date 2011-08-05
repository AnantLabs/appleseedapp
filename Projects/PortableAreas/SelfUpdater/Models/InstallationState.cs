using NuGet;
using System;
using System.Runtime.CompilerServices;


namespace SelfUpdater.Models
{

    public class InstallationState
    {
        [CompilerGenerated]
        private IPackage Installed_k__BackingField;
        [CompilerGenerated]
        private IPackage Update_k__BackingField;

        public IPackage Installed
        {
            [CompilerGenerated]
            get
            {
                return this.Installed_k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.Installed_k__BackingField = value;
            }
        }

        public IPackage Update
        {
            [CompilerGenerated]
            get
            {
                return this.Update_k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.Update_k__BackingField = value;
            }
        }

        public bool Scheduled { get; set; }
    }
}

