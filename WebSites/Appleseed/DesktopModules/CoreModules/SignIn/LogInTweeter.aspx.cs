using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twitterizer;
using System.Web.Security;


using System.Text;
using System.Text.RegularExpressions;  // This is for password validation
using System.Security.Cryptography;
using Appleseed.Framework.Security;
using Appleseed.Framework;
using System.Web.Profile;  // This is where the hash functions reside


namespace Appleseed.DesktopModules.CoreModules.SignIn
{
    public partial class LogInTweeter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TwitterAppId"] != null && Session["TwitterAppSecret"] != null) {
                string consumerKey = Session["TwitterAppId"] as string;
                string consumerSecret = Session["TwitterAppSecret"] as string;
                OAuthTokenResponse accessTokenResponse = OAuthUtility.GetAccessToken(consumerKey, consumerSecret,
                                                                                        Request.QueryString["oauth_token"],
                                                                                        Request.QueryString["oauth_verifier"]);

                Session["CameFromSocialNetwork"] = true;

                string userName = "Twitter_" + accessTokenResponse.ScreenName;
                string password = GeneratePasswordHash(userName);

                if (Membership.GetUser(userName) == null) {
                    //The user doesnt exists, needs to be registered

                    Session["TwitterUserName"] = userName;
                    Session["TwitterPassword"] = password;
                    string urlRegister = ConvertRelativeUrlToAbsoluteUrl("~/DesktopModules/CoreModules/Register/Register.aspx");
                    Response.Redirect(urlRegister);


                } else {
                    Session.Contents.Remove("CameFromSocialNetwork");
                    Session.Remove("CameFromSocialNetwork");
                    PortalSecurity.SignOn(userName, password);

                }

            } else {

                string urlRegister = ConvertRelativeUrlToAbsoluteUrl("");
                Response.Redirect(urlRegister);

            }
                
            
            
        }
        
        public string GeneratePasswordHash(string thisPassword)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] tmpSource;
            byte[] tmpHash;
    
            tmpSource = ASCIIEncoding.ASCII.GetBytes(thisPassword); // Turn password into byte array
            tmpHash = md5.ComputeHash(tmpSource);

            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));  // X2 formats to hexadecimal
            }
            return sOutput.ToString();
        }

        public string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl)
        {

            if (Request.IsSecureConnection)

                return string.Format("https://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

            else

                return string.Format("http://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

        }
    }
}