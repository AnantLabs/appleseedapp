using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Security;
using System.Security.Cryptography;
using System.Text;
using Appleseed.Framework;

namespace Appleseed.DesktopModules.CoreModules.SignIn {
    public partial class LoginIn : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            var email = Session["UserName"] as string;
            PortalSecurity.SignOn(email, GeneratePasswordHash(email),false, HttpUrlBuilder.BuildUrl("~/"));

        }

        public string GeneratePasswordHash(string thisPassword) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] tmpSource;
            byte[] tmpHash;

            tmpSource = ASCIIEncoding.ASCII.GetBytes(thisPassword); // Turn password into byte array
            tmpHash = md5.ComputeHash(tmpSource);

            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++) {
                sOutput.Append(tmpHash[i].ToString("X2"));  // X2 formats to hexadecimal
            }
            return sOutput.ToString();
        }
    }
}