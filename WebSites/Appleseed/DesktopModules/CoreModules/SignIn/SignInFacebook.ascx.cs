using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook.Web;
using System.Web.Security;
using Appleseed.Framework.Site.Configuration;
using Facebook;
using Appleseed.Framework.Security;
using Appleseed.Framework;
using System.Web.Profile;
using blowery.Web.HttpCompress;
using Appleseed.Framework.UI.WebControls;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using System.Net.Mail;
using Appleseed.Framework.Helpers;
using System.Text;
using Appleseed.Framework.Settings;

namespace Appleseed.DesktopModules.CoreModules.SignIn
{
    public partial class SignInFacebook : SignInControl
    {
        #region Event handlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var facebookContext = GetFacebookWebContext();
            if (facebookContext != null)
            {
                appId.Value = PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString();
                if (facebookContext.IsAuthenticated())
                {
                    //Here is were i check if the user login via facebook
                    FacebookSignInMethod();
                }
            }
            else
            {
                //TODO: ocultar boton y mostrar warning
                loginfb_div.Visible = false;
                errfb.Visible = true;
            }

            bool hide = true;
            bool autocomplete = false;
            if (this.ModuleID == 0)
            {
                if (Settings.ContainsKey("MODULESETTINGS_SHOW_TITLE"))
                {
                    ((SettingItem<bool, CheckBox>)Settings["MODULESETTINGS_SHOW_TITLE"]).Value = false;
                }
                else
                {
                    Settings.Add("MODULESETTINGS_SHOW_TITLE", new SettingItem<bool, CheckBox>());
                }
            }

            if (Settings.ContainsKey("SIGNIN_AUTOMATICALLYHIDE"))
            {
                hide = bool.Parse(Settings["SIGNIN_AUTOMATICALLYHIDE"].ToString());
            }

            if (Settings.ContainsKey("SIGNIN_ALLOW_AUTOCOMPLETE"))
            {
                autocomplete = bool.Parse(Settings["SIGNIN_ALLOW_AUTOCOMPLETE"].ToString());
            }

            if (Settings.ContainsKey("SIGNIN_ALLOW_REMEMBER_LOGIN"))
            {
                chkSaveLogin.Visible = bool.Parse(Settings["SIGNIN_ALLOW_REMEMBER_LOGIN"].ToString());
            }

            if (hide && Request.IsAuthenticated)
            {
                this.Visible = false;
            }
            else if (!autocomplete)
            {
                //New setting on Signin fo disable IE autocomplete by Mike Stone
                txtPassword.Attributes.Add("autocomplete", "off");
            }
        }

        private void UpdateProfile()
        {
            var client = new FacebookWebClient();
            dynamic me = client.Get("me");

            ProfileManager.Provider.ApplicationName = PortalSettings.PortalAlias;
            ProfileBase profile = ProfileBase.Create(me.email);
            profile.SetPropertyValue("Email", me.email);
            profile.SetPropertyValue("Name", me.name);
            try
            {
                profile.Save();
            }
            catch (Exception exc)
            {
                ErrorHandler.Publish(LogLevel.Error, "Error al salvar un perfil", exc);
            }
        }

        protected void valExistsEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            MembershipUser user = Membership.GetUser(txtEmail.Text);
            args.IsValid = user != null;
        }

        protected void valPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            valExistsEmail.Validate();
            if (valExistsEmail.IsValid)
            {
                args.IsValid = Membership.ValidateUser(txtEmail.Text, txtPassword.Text);
            }
        }

        protected void imgLogin_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                Session["PersistedUser"] = chkSaveLogin.Checked;
                if (PortalSecurity.SignOn(txtEmail.Text, txtPassword.Text, chkSaveLogin.Checked) == null)
                {
                    lblError.Visible = true;
                }
            }
        }

        protected void lnkRegister_Click(object sender, EventArgs e)
        {
            string url = HttpUrlBuilder.BuildUrl("~/DesktopModules/HealTheMatrixModules/Register/Register.aspx");
            Response.Redirect(url, true);
        }

        #endregion

        #region Properties

        public override Guid GuidID
        {
            get
            {
                return new Guid("{008D65E2-FB7E-4B00-BC2E-7491388BACDE}");
            }
        }

        #endregion

        protected void lbtnSendPassword_Click(object sender, EventArgs e)
        {
            if (this.txtEmail.Text == string.Empty)
            {
                this.lblPassSentErr.Text = "You must specify en email.";
                this.lblPassSentErr.Visible = true;
                return;
            }

            MembershipUser user = Membership.GetUser(this.txtEmail.Text.Trim());
            if (user != null)
            {
                string pwd = user.GetPassword();

                string msg = "Su contraseña es " + pwd;
                string from = Convert.ToString(PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"]);
                string subject = "Env&iacute;o de contraseña";
                string description = "Env&iacute;o de contraseña";

                SendMail(this.txtEmail.Text, from, subject, msg, description);

                lblPassSent.Visible = true;
            }
        }

        private void SendMail(string to, string from, string subject, string content, string description)
        {
            var mail = new MailMessage();

            // we check the PortalSettings in order to get if it has an sender registered 
            if (this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"] != null)
            {
                var sf = this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"];
                var mailFrom = sf.ToString();
                try
                {
                    mail.From = new MailAddress(mailFrom);
                }
                catch
                {
                    // if the address is not well formed, a warning is logged.
                    LogHelper.Logger.Log(
                        LogLevel.Warn,
                        string.Format(
                            @"This is the current email address used as sender when someone want to retrieve his/her password: '{0}'. 
Is not well formed. Check the setting SITESETTINGS_ON_REGISTER_SEND_FROM of portal '{1}' in order to change this value (it's a portal setting).",
                            mailFrom,
                            this.PortalSettings.PortalAlias));
                }
            }

            // if there is not a correct email in the portalSettings, we use the default sender specified on the web.config file in the mailSettings tag.
            mail.To.Add(new MailAddress(this.txtEmail.Text));
            mail.Subject = string.Format(
                "{0} - {1}",
                this.PortalSettings.PortalName,
                General.GetString("SIGNIN_PWD_LOST", "I lost my password", this));

            var sb = new StringBuilder();

            sb.Append("\r\n\r\n");
            sb.Append(
                General.GetString(
                    "SIGNIN_PWD_LOST_REQUEST_RECEIVED",
                    "We received your request regarding the loss of your password.",
                    this));
            sb.Append("\r\n");
            /*sb.Append(
                General.GetString(
                    "SIGNIN_SET_NEW_PWD_MSG",
                    "You can set a new password for your account going to the following link:",
                    this));*/
            sb.Append(content);
           // sb.Append(changePasswordUrl);
            sb.Append("\r\n\r\n");
            sb.Append(General.GetString("SIGNIN_THANK_YOU", "Thanks for your visit.", this));
            sb.Append(" ");
            sb.Append(this.PortalSettings.PortalName);
            sb.Append("\r\n\r\n");
            /*sb.Append(
                General.GetString(
                    "SIGNIN_URL_WARNING",
                    "NOTE: The address above may not show up on your screen as one line. This would prevent you from using the link to access the web page. If this happens, just use the 'cut' and 'paste' options to join the pieces of the URL.",
                    this));*/

            mail.Body = sb.ToString();
            mail.IsBodyHtml = false;

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Send(mail);

                    this.lblPassSentErr.Text = General.GetString(
                        "SIGNIN_PWD_WAS_SENT", "Your password was sent to the address you provided", this);
                    this.lblPassSentErr.Visible = true;
                }
                catch (Exception exception)
                {
                    this.lblPassSentErr.Text = General.GetString(
                        "SIGNIN_SMTP_SENDING_PWD_MAIL_ERROR",
                        "We can't send you your password. There were problems while trying to do so.");
                    this.lblPassSentErr.Visible = true;
                    LogHelper.Logger.Log(
                        LogLevel.Error,
                        string.Format(
                            "Error while trying to send the password to '{0}'. Perhaps you should check your SMTP server configuration in the web.config.",
                            this.txtEmail.Text),
                        exception);
                }
            }
        }
        #region Facebook Methods

        /// <summary>
        /// check if facebook settings were setting up from the portal settings, if not update the facebooksettings section of the web config file
        /// </summary>
        internal FacebookWebContext GetFacebookWebContext()
        {
            if (PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_ID") &&
                !PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString().Equals(string.Empty) &&
                PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_SECRET") &&
                !PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_SECRET"].ToString().Equals(string.Empty))
            {
                string appId = PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString();
                var appSecret = PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_SECRET"].ToString();

                if (FacebookWebContext.Current.Settings != null)
                {
                    var facebookConfigurationSection = new FacebookConfigurationSection();
                    facebookConfigurationSection.AppId = appId;
                    facebookConfigurationSection.AppSecret = appSecret;
                    return new FacebookWebContext(facebookConfigurationSection);
                }
            }

            return null;
        }

        private void FacebookSignInMethod()
        {
            var client = new FacebookWebClient();
            dynamic me = client.Get("me");
            var passwd = me.id;
            if (Membership.GetUser(me.email) == null)
            {
                MembershipCreateStatus status = MembershipCreateStatus.Success;
                MembershipUser user = Membership.Provider.CreateUser(me.email, passwd, me.email, "question", "answer", true, Guid.NewGuid(), out status);
                this.lblError.Text = string.Empty;

                switch (status)
                {
                    case MembershipCreateStatus.DuplicateEmail:
                    case MembershipCreateStatus.DuplicateUserName:
                        this.lblError.Text = "El usuario ya existe";
                        break;
                    case MembershipCreateStatus.ProviderError:
                        break;
                    case MembershipCreateStatus.Success:
                        UpdateProfile();
                        if (PortalSecurity.SignOn(me.email, passwd) == null)
                        {
                            lblError.Text = "Facebook loggin fail.";
                            lblError.Visible = true;
                        }
                        break;
                    //Todos los otros...
                    default:
                        this.lblError.Text = "Ocurri&oacute;ó un error al salvar su usuario. Intente nuevamente.";
                        break;
                }
            }
            else
            {
                if (PortalSecurity.SignOn(me.email, passwd) == null)
                {
                    lblError.Text = "Facebook loggin fail.";
                    lblError.Visible = true;
                }

            }
        }

        public override void Logoff()
        {
            var context = GetFacebookWebContext();
            if (context != null && context.IsAuthenticated())
            {
                context.DeleteAuthCookie();
            }
        }

        #endregion Facebook Methods
    }
}