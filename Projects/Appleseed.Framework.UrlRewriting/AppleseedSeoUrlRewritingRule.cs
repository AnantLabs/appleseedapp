using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.Framework.UrlRewriting
{
    using System.Configuration;
    using System.Globalization;
    using System.Web;

    using UrlRewritingNet.Configuration;
    using UrlRewritingNet.Web;
    using System.Text.RegularExpressions;


    public class AppleseedSeoUrlRewritingRule : RewriteRule
    {
        #region Constants and Fields

        /// <summary>
        /// The default splitter.
        /// </summary>
        private string defaultSplitter = "__";

        /// <summary>
        /// The friendly page name.
        /// </summary>
        private string friendlyPageName = "Default.aspx";

        /// <summary>
        /// The handler flag.
        /// </summary>
        private string handlerFlag = "site";

        private Regex regex;

        #endregion

        public override void Initialize(RewriteSettings rewriteSettings)
        {
            base.Initialize(rewriteSettings);

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["handlerflag"])) {
                this.handlerFlag = rewriteSettings.Attributes["handlerflag"].ToLower(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["handlersplitter"])) {
                this.defaultSplitter = rewriteSettings.Attributes["handlersplitter"];
            } else {
                if (ConfigurationManager.AppSettings["HandlerDefaultSplitter"] != null) {
                    this.defaultSplitter = ConfigurationManager.AppSettings["HandlerDefaultSplitter"];
                }
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["pageidnosplitter"])) {
                bool.Parse(rewriteSettings.Attributes["pageidnosplitter"]);
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["friendlyPageName"])) {
                this.friendlyPageName = rewriteSettings.Attributes["friendlyPageName"];
            }

            //To match the url form (atr1/atr2/.../atrn)optional/number/name.aspx(?a=x&b=y&...&c=z#xxx)optional
            regex = new Regex("^(.*)/([0-9]*)/(.*)\\.aspx(\\?.*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        }

        public override bool IsRewrite(string requestUrl)
        {
            bool b = this.regex.IsMatch(requestUrl);
            return b;
        }


        public override string RewriteUrl(string url)
        {

            var parts = url.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

            // If it is an virtual app
            var path = HttpContext.Current.Request.ApplicationPath;
            var rewrittenUrl = "/";
            
            if (!path.Equals('/')) {
                rewrittenUrl += path;
                if (!path.EndsWith("/")) {
                    rewrittenUrl += "/";
                }
            }
            rewrittenUrl += string.Format("{0}", this.friendlyPageName);

            var pageId = "0"; //this is made in order to allow urls formed only with the handler (/site/ been the default). Those urls will be redirected to the portal home.
            if (parts.Length >= 2) {
                pageId = parts[parts.Length - 2];
            }
            var queryString = string.Format("?pageId={0}", pageId);

            if (parts.Length > 2) {
                for (var i = 0; i < parts.Length - 2; i++) {
                    var queryStringParam = parts[i];

                    if (queryStringParam.IndexOf(this.defaultSplitter) < 0) {
                        continue;
                    }

                    queryString += string.Format(
                        "&{0}",
                        queryStringParam.Substring(0, queryStringParam.IndexOf(this.defaultSplitter)));
                    queryString += string.Format(
                        "={0}",
                        queryStringParam.Substring(queryStringParam.IndexOf(this.defaultSplitter) + this.defaultSplitter.Length));
                }
            }

            //Agregar los query que haya en el ultimo, y el hash
            string last = parts[parts.Length - 1];
            // Hay algun atributo de query
            if(last.IndexOf('?') > 0){
                var queryparts = last.Split(new char[] { '&' }, System.StringSplitOptions.RemoveEmptyEntries);
                queryparts[0] = queryparts[0].Substring(1, queryparts[0].Length - 1);
                queryString += queryparts[0];
                // si query parts tiene mas de un &, tiene mas de un atributo
                if (queryparts.Length > 1) {
                    for (int i = 1; i < queryparts.Length - 1; i++) {
                        queryString += queryparts[i];
                    }
                }
    
            }
            
                     


            HttpContext.Current.RewritePath(rewrittenUrl, string.Empty, queryString);

            return rewrittenUrl + queryString;
        }


    }
}
