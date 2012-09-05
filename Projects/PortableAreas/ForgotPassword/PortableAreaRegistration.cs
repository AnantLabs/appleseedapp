using System.Web.Mvc;
using MvcContrib.PortableAreas;
using Appleseed.Core.ApplicationBus;
using Appleseed.Core;

namespace ForgotPassword {
    public class ForgotPasswordRegistration : MvcContrib.PortableAreas.PortableAreaRegistration {
        public override string AreaName {
            get {
                return "ForgotPassword";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus) {
            context.MapRoute("ForgotPassword_ResourceRoute", "ForgotPassword/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("ForgotPassword_ResourceImageRoute", "ForgotPassword/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute(
                "ForgotPassword_default",
                "ForgotPassword/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional }
            );

            this.RegisterAreaEmbeddedResources();
            PortableAreaUtils.RegisterScripts(this, context, bus);
        }

    }
}
