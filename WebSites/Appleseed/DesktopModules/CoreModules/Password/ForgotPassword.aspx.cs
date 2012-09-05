using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Page = Appleseed.Framework.Web.UI.Page;
using Resources;
using System.Web.Security;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using Appleseed.Framework;
using Appleseed.Framework.Settings;
using System.Net.Mail;
using Appleseed.Framework.Helpers;
using System.Text;


namespace Appleseed.DesktopModules.CoreModules.Password {
    public partial class ForgotPassword : Page {

        protected void Page_Load(object sender, EventArgs e) {

            if (this.Request.IsAuthenticated) {

                Response.Redirect("/");
                
            }

            EmailLabel.InnerText = "Ingresa tu email: ";
            SendPasswordBtn.Text = "Recuperar Contraseña";

            if (!string.IsNullOrEmpty(Request.QueryString["email"])) {
                this.email.Text = Request.QueryString["email"];
            }
        }

        protected override void OnInit(EventArgs e) {

            this.SendPasswordBtn.Click += this.SendPasswordBtnClick;
            

            base.OnInit(e);
        }

        private void SendPasswordBtnClick(object sender, EventArgs e) {
            
            if (this.email.Text == string.Empty) {
                this.Message.Text = Resources.Appleseed.Signin_SendPasswordBtnClick_Please_enter_you_email_address;
                this.Message.TextKey = "SIGNIN_ENTER_EMAIL_ADDR";
                return;
            }

            Membership.ApplicationName = this.PortalSettings.PortalAlias;
            var membership = (AppleseedMembershipProvider)Membership.Provider;

            // Obtain single row of User information
            var memberUser = membership.GetUser(this.email.Text, false);

            if (memberUser == null) {
                this.Message.Text = General.GetString(
                    "SIGNIN_PWD_MISSING_IN_DB", "The email you specified does not exists on our database", this);
                this.Message.TextKey = "SIGNIN_PWD_MISSING_IN_DB";
                return;
            }

            var userId = (Guid)(memberUser.ProviderUserKey ?? Guid.Empty);

            // generate Token for user
            var token = membership.CreateResetPasswordToken(userId);

            var changePasswordUrl = string.Concat(
                Path.ApplicationFullPath,
                "DesktopModules/CoreModules/Admin/ChangePassword.aspx?usr=",
                userId.ToString("N"),
                "&tok=",
                token.ToString("N"));

            var mail = new MailMessage();

            // we check the PortalSettings in order to get if it has an sender registered 
            if (this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"] != null) {
                var sf = this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"];
                var mailFrom = sf.ToString();
                try {
                    mail.From = new MailAddress(mailFrom);
                }
                catch {
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
            mail.To.Add(new MailAddress(this.email.Text));
            mail.Subject = string.Format(
                "{0} - {1}",
                this.PortalSettings.PortalName,
                General.GetString("SIGNIN_PWD_LOST", "I lost my password", this));

            var sb = new StringBuilder();

            sb.Append(memberUser.UserName);
            sb.Append(",");
            sb.Append("\r\n\r\n");
            sb.Append(
                General.GetString(
                    "SIGNIN_PWD_LOST_REQUEST_RECEIVED",
                    "We received your request regarding the loss of your password.",
                    this));
            sb.Append("\r\n");
            sb.Append(
                General.GetString(
                    "SIGNIN_SET_NEW_PWD_MSG",
                    "You can set a new password for your account going to the following link:",
                    this));
            sb.Append(" ");
            sb.Append(changePasswordUrl);
            sb.Append("\r\n\r\n");
            sb.Append(General.GetString("SIGNIN_THANK_YOU", "Thanks for your visit.", this));
            sb.Append(" ");
            sb.Append(this.PortalSettings.PortalName);
            sb.Append("\r\n\r\n");
            sb.Append(
                General.GetString(
                    "SIGNIN_URL_WARNING",
                    "NOTE: The address above may not show up on your screen as one line. This would prevent you from using the link to access the web page. If this happens, just use the 'cut' and 'paste' options to join the pieces of the URL.",
                    this));

            mail.Body = sb.ToString();
            mail.IsBodyHtml = false;

            using (var client = new SmtpClient()) {
                try {
                    client.Send(mail);

                    this.Message.Text = General.GetString(
                        "SIGNIN_PWD_WAS_SENT", "Your password was sent to the address you provided", this);
                    this.Message.TextKey = "SIGNIN_PWD_WAS_SENT";

                    this.EmailLabel.Visible = false;
                    this.email.Visible = false;
                    this.SendPasswordBtn.Visible = false;

                }
                catch (Exception exception) {
                    this.Message.Text = General.GetString(
                        "SIGNIN_SMTP_SENDING_PWD_MAIL_ERROR",
                        "We can't send you your password. There were problems while trying to do so.");
                    this.Message.TextKey = "SIGNIN_SMTP_SENDING_PWD_MAIL_ERROR";
                    LogHelper.Logger.Log(
                        LogLevel.Error,
                        string.Format(
                            "Error while trying to send the password to '{0}'. Perhaps you should check your SMTP server configuration in the web.config.",
                            this.email.Text),
                        exception);
                }
            }
        }
    }
}