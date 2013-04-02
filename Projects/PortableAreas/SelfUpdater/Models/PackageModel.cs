using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfUpdater.Models
{
    public class PackageModel
    {

        public string Name { get; set; }
        public string Source { get; set; }
        public string Version { get; set; }
        public bool Install { get; set; }


    }

    public class PackageModalModel
    {
        public bool ShowModal { get; set; }

        public List<dynamic> Packages { get; set; }
    }
}