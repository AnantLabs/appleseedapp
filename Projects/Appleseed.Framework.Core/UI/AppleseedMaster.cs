using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
using Appleseed.Framework.Site.Configuration;
using System.Web.Mvc;

namespace Appleseed
{
    public class AppleseedMaster : System.Web.Mvc.ViewMasterPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            var scripts = GetBaseScripts();

            int index = 0;
            foreach (var script in scripts)
            {
                HtmlGenericControl include = new HtmlGenericControl("script");
                include.Attributes.Add("type", "text/javascript");
                include.Attributes.Add("src", script as string);
                this.Page.Header.Controls.AddAt(index++, include);
            }



            var portalSettings = (PortalSettings)Context.Items["PortalSettings"];

            if (portalSettings != null)
            {
                var cssHref = "/Design/jqueryUI/" + portalSettings.PortalAlias + "/jquery-ui.custom.css";

                HtmlGenericControl include = new HtmlGenericControl("link");
                include.Attributes.Add("type", "text/css");
                include.Attributes.Add("rel", "stylesheet");
                include.Attributes.Add("href", cssHref);
                this.Page.Header.Controls.AddAt(index++, include);
            }

            var uiculture = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var datepickerscript = "$(document).ready(function(){$.datepicker.setDefaults($.datepicker.regional['" + uiculture + "']);});";

            HtmlGenericControl includedp = new HtmlGenericControl("script");
            includedp.Attributes.Add("type", "text/javascript");
            includedp.InnerHtml = datepickerscript;
            this.Page.Header.Controls.AddAt(index++, includedp);

            base.OnLoad(e);
        }

        public static ArrayList GetBaseScripts()
        {
            var scripts = new ArrayList();

            scripts.Add("/aspnet_client/jQuery/jquery-1.6.1.min.js");
            scripts.Add("/aspnet_client/jQuery/jquery-ui-1.8.11.min.js");
            scripts.Add("/aspnet_client/jQuery/jquery.validate.min.js");
            scripts.Add("/aspnet_client/jQuery/jquery.validate.unobtrusive.min.js");
            scripts.Add("/aspnet_client/jQuery/jquery.bgiframe.min.js");
            scripts.Add("/aspnet_client/jQuery/jquery-ui-i18n.min.js");
            scripts.Add("/aspnet_client/jQuery/jquery.unobtrusive-ajax.min.js");

            scripts.Add("/aspnet_client/jQuery/modernizr-1.7.min.js");
			scripts.Add("/aspnet_client/jQuery/jquery.cookie.js");

            scripts.Add("/aspnet_client/jQuery/MicrosoftAjax.js");
            scripts.Add("/aspnet_client/jQuery/MicrosoftMvcAjax.js");
            scripts.Add("/aspnet_client/jQuery/MicrosoftMvcValidation.js");

            scripts.Add("/aspnet_client/js/DragNDrop.js");
            scripts.Add("/aspnet_client/js/browser_upgrade_notification.js");

            scripts.Add("/aspnet_client/CSSControlAdapters/AdapterUtils.js");
            scripts.Add("/aspnet_client/CSSControlAdapters/MenuAdapter.js");
            return scripts;
        }
    }
}