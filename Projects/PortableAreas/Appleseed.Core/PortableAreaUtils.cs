using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Appleseed.Core.ApplicationBus;
using System.Reflection;
using MvcContrib.PortableAreas;

namespace Appleseed.Core
{
    public static class PortableAreaUtils
    {

        public static void RegisterScripts(this PortableAreaRegistration portableArea, System.Web.Mvc.AreaRegistrationContext context, MvcContrib.PortableAreas.IApplicationBus bus)
        {
            bus.Send(new BusMessage { Message = portableArea.AreaName + " registered" });

            bus.Send(new DBScriptsMessage
            {
                AreaName = portableArea.AreaName,
                LastVersion = GetLastDBScriptVersion(),
                Scripts = GetScripts()

            });
        }

        private static List<DBScriptDescriptor> GetScripts()
        {
            string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            var result = new List<DBScriptDescriptor>();

            foreach (var resource in resources.Where(d => d.Contains(".sql")))
            {
                var version = resource.Substring(resource.IndexOf("._") + 2, 11).Replace(".", "_");

                result.Add(new DBScriptDescriptor { Url = resource, Version = version });
            }

            return result;
        }

        private static string GetLastDBScriptVersion()
        {
            string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            //eg: Appleseed.Core.DBScripts._20110413.01. Create_DBVersion_Table.sql

            var dbversions = resources.Where(d => d.Contains(".sql")).Select(d => d.Substring(d.IndexOf("._") + 2, 11));

            return dbversions.OrderBy(d => d).LastOrDefault();
        }
    }
}