
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;

namespace Appleseed
{
    public class AppleseedMaster : MasterPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            var scripts = new ArrayList();


            scripts.Add("/aspnet_client/jQuery/jquery-1.4.4.min.js");

            scripts.Add("/aspnet_client/jQuery/jquery-ui-1.8.7.min.js");
            scripts.Add("/aspnet_client/jQuery/jquery.validate.min.js");
            scripts.Add("/aspnet_client/jQuery/jquery.bgiframe.js");

            scripts.Add("/aspnet_client/jQuery/jquery-ui-i18n.min.js");
            scripts.Add("/aspnet_client/js/DragNDrop.js");
            scripts.Add("/aspnet_client/js/browser_upgrade_notification.js");

            scripts.Add("/aspnet_client/CSSControlAdapters/AdapterUtils.js");
            scripts.Add("/aspnet_client/CSSControlAdapters/MenuAdapter.js");

            int index = 0;
            foreach (var script in scripts)
            {
                HtmlGenericControl include = new HtmlGenericControl("script");
                include.Attributes.Add("type", "text/javascript");
                include.Attributes.Add("src", script as string);
                this.Page.Header.Controls.AddAt(index++, include);
            }

            base.OnLoad(e);
        }
    }
}