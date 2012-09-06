using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appleseed.Framework;
using Password.Models;
using System.Web.Security;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using Appleseed.Framework.Site.Configuration;

namespace Password.Controllers
{
    public class PasswordRecoveryController : Controller
    {
        //
        // GET: /PasswordRecovery/

        protected PortalSettings PortalSettings {
            get {
                return (PortalSettings)HttpContext.Items["PortalSettings"];
            }
        }

        public ActionResult Index()
        {

            if (Request.IsAuthenticated) {
                Response.Redirect("/");
            }
            
            var model = new RecoveryModel();
            if (Request.QueryString["usr"] == null || Request.QueryString["tok"] == null) {
                model.message = General.GetString("CHANGE_PWD_INVALID_URL_ERROR", "You are not allowed to use this functionality witout the correct parameters.");
                model.error = true;
                return View(model);
            }

            Guid userId;
            Guid token;

            if (!Guid.TryParse(Request.QueryString["usr"].ToString(), out userId) ||
                !Guid.TryParse(Request.QueryString["tok"].ToString(), out token)) {
                model.message = General.GetString("CHANGE_PWD_INVALID_URL_ERROR", "You are not allowed to use this functionality witout the correct parameters.");
                model.error = true;
                return View(model);
            }

           
            Membership.ApplicationName = this.PortalSettings.PortalAlias;
            var membership = (AppleseedMembershipProvider)Membership.Provider;
            if (!membership.VerifyTokenForUser(userId, token)) {
                model.message = General.GetString("CHANGE_PWD_INVALID_URL_ERROR", "You are not allowed to use this functionality witout the correct parameters.");
                model.error = true;
                return View(model);
            }

            model.message = General.GetString("CHANGE_PWD_USR_EXPLANATION", "Insert your new password for your account. Once you save the changes you would be able to logon with it.");
            

            return View(model);
        }

        
    }
}
