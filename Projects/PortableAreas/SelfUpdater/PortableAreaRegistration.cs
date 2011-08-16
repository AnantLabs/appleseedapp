using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace SelfUpdater
{
    public class SelfUpdaterRegistration : MvcContrib.PortableAreas.PortableAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SelfUpdater";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            context.MapRoute("SelfUpdater_ResourceRoute", "SelfUpdater/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("SelfUpdater_ResourceImageRoute", "SelfUpdater/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("SelfUpdater.Core_ResourceScriptRoute", "SelfUpdater/scripts/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index", resourcePath = "Scripts" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute(
                "SelfUpdater_default",
                "SelfUpdater/{controller}/{action}/{id}",
                new { action = "Index", controller = "Updates", id = UrlParameter.Optional }
            );

            this.RegisterAreaEmbeddedResources();
        }

    }
}
