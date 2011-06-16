using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
using Appleseed.Framework.Site.Configuration;
using System.Web.Mvc;
using System.Web;
using System.Xml.Linq;
using System;
using Appleseed.Framework;

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
            InsertAllScripts(Page, Context);

            base.OnLoad(e);
        }

        public static void InsertAllScripts(Page page, HttpContext context)
        {
            if (!page.ClientScript.IsClientScriptBlockRegistered("allscripts")) {
                var scripts = GetBaseScripts();

                int index = 0;
                foreach (var script in scripts) {
                    HtmlGenericControl include = new HtmlGenericControl("script");
                    include.Attributes.Add("type", "text/javascript");
                    include.Attributes.Add("src", script as string);
                    page.Header.Controls.AddAt(index++, include);
                }
                
                var portalSettings = (PortalSettings)context.Items["PortalSettings"];

                if (portalSettings != null) {
                    var cssHref = "/Design/jqueryUI/" + portalSettings.PortalAlias + "/jquery-ui.custom.css";

                    HtmlGenericControl include = new HtmlGenericControl("link");
                    include.Attributes.Add("type", "text/css");
                    include.Attributes.Add("rel", "stylesheet");
                    include.Attributes.Add("href", cssHref);
                    page.Header.Controls.AddAt(index++, include);
                }

                var uiculture = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                var datepickerscript = "$(document).ready(function(){$.datepicker.setDefaults($.datepicker.regional['" + uiculture + "']);});";

                HtmlGenericControl includedp = new HtmlGenericControl("script");
                includedp.Attributes.Add("type", "text/javascript");
                includedp.InnerHtml = datepickerscript;
                page.Header.Controls.AddAt(index++, includedp);


                string extraScripts = GetExtraScripts();
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "allscripts", extraScripts, false);
            }
        }

        public static string GetExtraScripts()
        {
            string scripts = string.Empty;
            try {
                string filePath = HttpContext.Current.Server.MapPath("~/Scripts/Scripts.xml");
                XDocument xml = XDocument.Load(filePath);
                foreach (var s in xml.Descendants("scripts").DescendantNodes()) {
                    scripts += s.ToString() + Environment.NewLine;
                }
            } catch (Exception exc) {
                ErrorHandler.Publish(LogLevel.Debug, exc);
            }

            return scripts;
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