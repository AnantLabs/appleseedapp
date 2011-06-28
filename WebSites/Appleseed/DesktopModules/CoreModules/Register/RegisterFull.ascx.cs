using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Appleseed.Framework;
using Appleseed.Framework.Providers.Geographic;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;
using System.Security.Cryptography;


public partial class DesktopModules_CoreModules_Register_RegisterFull : PortalModuleControl, IEditUserProfile
{

    private string _redirectPage;
    private DateTime _defaultRegisterDate = new DateTime(DateTime.Today.Year, 1, 1);

    /// <summary>
    /// ddlDay control.
    /// </summary>
    /// <remarks>
    /// </remarks>
    protected global::System.Web.UI.WebControls.DropDownList ddlDay;

    /// <summary>
    /// ddlMonth control.
    /// </summary>
    /// <remarks>
    /// </remarks>
    protected global::System.Web.UI.WebControls.DropDownList ddlMonth;

    /// <summary>
    /// ddlYear control.
    /// </summary>
    /// <remarks>
    /// </remarks>
    protected global::System.Web.UI.WebControls.DropDownList ddlYear;


    private bool OuterCreation
    {
        get
        {
            if (Request.Params["outer"] != null)
            {
                return Convert.ToInt32(Request.Params["outer"]) == 1 ? true : false;
            }
            else
            {
                return false;
            }
        }
    }

    protected override void OnInit(EventArgs e)
    {
        recaptcha.PublicKey = Convert.ToString(this.PortalSettings.CustomSettings["SITESETTINGS_RECAPTCHA_PUBLIC_KEY"]);
        recaptcha.PrivateKey = Convert.ToString(this.PortalSettings.CustomSettings["SITESETTINGS_RECAPTCHA_PRIVATE_KEY"]);
        recaptcha.Language = this.PortalSettings.PortalContentLanguage.TwoLetterISOLanguageName;
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadBirthDateControls();
        trPwd.Visible = false;
        trPwdAgain.Visible = false;
        lnkChangePassword.Visible = false;
        panChangePwd.Visible = false;
        ViewState["responseWithPopup"] = null;

        //captcha will only be displayed if the user is not authenticated.
        trCaptcha.Visible = !Context.User.Identity.IsAuthenticated;

        if (!EditMode)
        {
            trPwd.Visible = true;
            trPwdAgain.Visible = true;
        }
        else
        {
            lnkChangePassword.Visible = true;
            panChangePwd.Visible = true;
        }
        if (!Page.IsPostBack)
        {

            BindCountry();
            BindDateCombos();
            this.lblError.Text = string.Empty;
            this.lblSuceeded.Text = string.Empty;
            this.pnlSuceeded.Visible = false;
            this.pnlForm.Visible = true;

            this.btnSave.Visible = !this.Request.FilePath.Contains("UsersManage.aspx");

            if (Request.QueryString["email"] != null)
            {
                tfEmail.Text = Request.QueryString["email"];
            }


            if (EditMode && !OuterCreation)
            {
                lblTitle.Text = (string)GetGlobalResourceObject("Appleseed", "USER_MODIFICATION");
            }
            else
            {
                lblTitle.Text = (string)GetGlobalResourceObject("Appleseed", "USER_REGISTRY");

                if (OuterCreation)
                {
                    lblSendNotification.Visible = true;
                    chbSendNotification.Visible = true;
                    chbSendNotification.Checked = true;
                }
            }

            if (EditMode)
            {
                var profileCommon = ProfileBase.Create(UserName);
                if (profileCommon != null)
                {
                    if ((DateTime)profileCommon.GetPropertyValue("BirthDate") == DateTime.MinValue)
                    {
                        BirthdayField = _defaultRegisterDate;
                    }
                    else
                    {
                        BirthdayField = (DateTime)profileCommon.GetPropertyValue("BirthDate");
                    }
                    this.tfCompany.Text = (string)profileCommon.GetPropertyValue("Company");
                    try
                    {
                        this.ddlCountry.SelectedValue = (string)profileCommon.GetPropertyValue("CountryID");
                    }
                    catch (Exception exc)
                    {
                        ErrorHandler.Publish(LogLevel.Error, Resources.Appleseed.PROFILE_COUNTRY_WRONG_ID, exc);
                    }
                    if (((string)profileCommon.GetPropertyValue("Email")).Equals(string.Empty))
                        this.tfEmail.Text = UserName;
                    else
                        this.tfEmail.Text = (string)profileCommon.GetPropertyValue("Email");
                    //if (!UserName.Equals(this.tfEmail.Text) || ((string)profileCommon.GetPropertyValue("Password")).Equals(GeneratePasswordHash(UserName))) { 
                    //    trPwd.Visible = false;
                    //    trPwdAgain.Visible = false;
                    //    lnkChangePassword.Visible = false;
                    //    panChangePwd.Visible = false;
                    //}
                    this.tfEmail.Enabled = false;
                    this.tfName.Text = (string)profileCommon.GetPropertyValue("Name");
                    this.tfPhone.Text = (string)profileCommon.GetPropertyValue("Phone");
                    this.chbReceiveNews.Checked = (bool)profileCommon.GetPropertyValue("SendNewsletter");
                }
            }
            else
            {
                var firstOptionText = General.GetString("REGISTER_SELECT_COUNTRY", "Select Country", this);
                this.ddlCountry.Items.Insert(0, new ListItem(string.Concat("-- ", firstOptionText), string.Empty));
                BirthdayField = _defaultRegisterDate;
            }
        }
        if (Session["CameFromSocialNetwork"] != null) {
            if ((bool)Session["CameFromSocialNetwork"]) {
                trPwd.Visible = false;
                trPwdAgain.Visible = false;
                lnkChangePassword.Visible = false;
                panChangePwd.Visible = false;
            }
        }
        if (Session["FacebookUserName"] != null) {
            this.tfEmail.Text = (string)Session["FacebookUserName"];
            this.tfEmail.ReadOnly = true;

        }
        if (Session["FacebookName"] != null) {
            this.tfName.Text = (string)Session["FacebookName"];
        }
    }

    private void LoadBirthDateControls()
    {
        ddlDay = new DropDownList();
        ddlDay.ID = "ddlDay";
        ddlMonth = new DropDownList();
        ddlMonth.ID = "ddlMonth";
        ddlYear = new DropDownList();
        ddlYear.ID = "ddlYear";

        var datePattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
        if (datePattern.IndexOf("M") < datePattern.IndexOf("d"))
        {
            plhBirthDate.Controls.Add(ddlMonth);
            plhBirthDate.Controls.Add(ddlDay);
        }
        else
        {
            plhBirthDate.Controls.Add(ddlDay);
            plhBirthDate.Controls.Add(ddlMonth);
        }
        plhBirthDate.Controls.Add(ddlYear);
    }

    private DateTime BirthdayField
    {
        set
        {
            if (value > DateTime.MinValue)
            {
                this.ddlDay.SelectedValue = value.Day.ToString();
                this.ddlMonth.SelectedValue = value.Month.ToString();
                this.ddlYear.SelectedValue = value.Year.ToString();
            }
        }
        get
        {
            int day = Convert.ToInt32(this.ddlDay.SelectedValue);
            int month = Convert.ToInt32(this.ddlMonth.SelectedValue);
            int year = Convert.ToInt32(this.ddlYear.SelectedValue);

            //if the the day of month is greater than the quantity of days of that month, then we set the total days of that months.
            //i.e. If the month is June, and the day is the 31th, then we set the 30th instead.
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day > daysInMonth)
            {
                day = daysInMonth;
            }
            return new DateTime(year, month, day);
        }
    }

    private void BindDateCombos()
    {
        List<int> days = new List<int>();
        for (int i = 1; i <= 31; i++)
        {
            days.Insert(i - 1, i);
        }

        ddlDay.DataSource = days;
        ddlDay.DataBind();

        var months = new Dictionary<int, string>();
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        for (int j = 1; j <= 12; j++)
        {
            months.Add(j, j.ToString() + " - " + textInfo.ToTitleCase(DateTimeFormatInfo.CurrentInfo.GetMonthName(j)));
        }

        ddlMonth.DataSource = months;
        ddlMonth.DataValueField = "key";
        ddlMonth.DataTextField = "value";
        ddlMonth.DataBind();

        List<int> years = new List<int>();
        for (int k = 1900; k <= DateTime.Today.Year; k++)
        {
            years.Insert(k - 1900, k);
        }

        ddlYear.DataSource = years;
        ddlYear.DataBind();
    }


    protected void cvCaptcha_ServerValidate(object source, ServerValidateEventArgs args)
    {
        recaptcha.Validate();
        args.IsValid = recaptcha.IsValid;
    }

    protected void cvCurrentPwdCorrect_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = Membership.Provider.ValidateUser(UserName, txtCurrentPwd.Text);
    }




    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveUserData();
        }
    }

    protected void btnChangePwd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                Membership.Provider.ChangePassword(UserName, txtCurrentPwd.Text, txtNewPwd.Text);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        else
        {
            ViewState["responseWithPopup"] = true;
        }
    }

    private string UserName
    {
        get
        {
            string uid = string.Empty;

            if (Request.Params["userName"] != null) {
                uid = Request.Params["userName"];
            }
            
            if(uid.Length == 0 && PortalSettings.CurrentUser != null && PortalSettings.CurrentUser.Identity != null)
                    uid = PortalSettings.CurrentUser.Identity.UserName;

            if (uid.Length == 0 && HttpContext.Current.Items["userName"] != null) {
                uid = HttpContext.Current.Items["userName"].ToString();
            }
            return uid;
        }
    }

    #region IEditUserProfile Members

    public bool EditMode
    {
        get { return (UserName.Length != 0); }
    }

    public string RedirectPage
    {
        get
        {
            if (_redirectPage == null)
            {
                return Request.Url.Segments[Request.Url.Segments.Length - 1] + "?TabID=" + PageID + "&mID=" + ModuleID + "&username=" + this.tfEmail.Text;
            }
            return _redirectPage;
        }
        set
        {
            _redirectPage = value;
        }
    }

    public Guid SaveUserData()
    {
        if (!EditMode) {
            Guid result = Guid.Empty;
            if (Session["CameFromSocialNetwork"] == null) {
                MembershipCreateStatus status = MembershipCreateStatus.Success;
                MembershipUser user = Membership.Provider.CreateUser(tfEmail.Text, tfPwd.Text, tfEmail.Text, "question", "answer", true, Guid.NewGuid(), out status);
                this.lblError.Text = string.Empty;

                switch (status) {
                    case MembershipCreateStatus.DuplicateEmail:
                    case MembershipCreateStatus.DuplicateUserName:
                        this.lblError.Text = Resources.Appleseed.USER_ALREADY_EXISTS;
                        break;
                    case MembershipCreateStatus.ProviderError:
                        break;
                    case MembershipCreateStatus.Success:
                        UpdateProfile();
                        result = (Guid)user.ProviderUserKey;
                        //if the user is registering himself (thus, is not yet authenticated) we will sign him on and send him to the home page.
                        if (!Context.User.Identity.IsAuthenticated) {
                            PortalSecurity.SignOn(tfEmail.Text, tfPwd.Text, false, HttpUrlBuilder.BuildUrl());
                        }
                        break;
                    // for every other error message...
                    default:
                        this.lblError.Text = Resources.Appleseed.USER_SAVING_ERROR;
                        break;
                }
                return result;
            } else {

                if (Session["TwitterUserName"] != null) {
                    // Register Twitter
                    string userName = (string)Session["TwitterUserName"];
                    string password = (string)Session["TwitterPassword"];
                    MembershipCreateStatus status = MembershipCreateStatus.Success;
                    MembershipUser user = Membership.Provider.CreateUser(userName, password, tfEmail.Text, "question", "answer", true, Guid.NewGuid(), out status);
                    this.lblError.Text = string.Empty;

                    switch (status) {
                        case MembershipCreateStatus.DuplicateEmail:
                        case MembershipCreateStatus.DuplicateUserName:
                            this.lblError.Text = Resources.Appleseed.USER_ALREADY_EXISTS;
                            break;
                        case MembershipCreateStatus.ProviderError:
                            break;
                        case MembershipCreateStatus.Success:
                            UpdateProfile();
                            result = (Guid)user.ProviderUserKey;
                            //if the user is registering himself (thus, is not yet authenticated) we will sign him on and send him to the home page.
                            if (!Context.User.Identity.IsAuthenticated) {
                                Session.Contents.Remove("CameFromSocialNetwork");
                                PortalSecurity.SignOn(userName, password, false, HttpUrlBuilder.BuildUrl());
                            }

                            break;
                        // for every other error message...
                        default:
                            this.lblError.Text = Resources.Appleseed.USER_SAVING_ERROR;
                            break;
                    }

                    return result;
                } else if (Session["FacebookUserName"] != null) {
                    // Register Facebook
                    string userName = (string)Session["FacebookUserName"];
                    string password = (string)Session["FacebookPassword"];
                    MembershipCreateStatus status = MembershipCreateStatus.Success;
                    MembershipUser user = Membership.Provider.CreateUser(userName, password, userName, "question", "answer", true, Guid.NewGuid(), out status);
                    this.lblError.Text = string.Empty;

                    switch (status) {
                        case MembershipCreateStatus.DuplicateEmail:
                        case MembershipCreateStatus.DuplicateUserName:
                            this.lblError.Text = Resources.Appleseed.USER_ALREADY_EXISTS;
                            break;
                        case MembershipCreateStatus.ProviderError:
                            break;
                        case MembershipCreateStatus.Success:
                            UpdateProfile();
                            result = (Guid)user.ProviderUserKey;
                            //if the user is registering himself (thus, is not yet authenticated) we will sign him on and send him to the home page.
                            if (!Context.User.Identity.IsAuthenticated) {
                                Session.Contents.Remove("CameFromSocialNetwork");
                                PortalSecurity.SignOn(userName, password, false, HttpUrlBuilder.BuildUrl());
                            }

                            break;
                        // for every other error message...
                        default:
                            this.lblError.Text = Resources.Appleseed.USER_SAVING_ERROR;
                            break;
                    }

                    return result;

                } else {
                    return result;
                }
            }
            

        } else {
            string Email = tfEmail.Text;
            string UserName = Membership.GetUserNameByEmail(Email);
            if (!UserName.Equals(Email)) {
                // The user Came from twitter
                Session["CameFromSocialNetwork"] = true;
                Session["TwitterUserName"] = UserName;
                Session["deleteCookies"] = true;
            }
            UpdateProfile();
            return (Guid)Membership.GetUser(UserName, false).ProviderUserKey;
        }
    }



   private void UpdateProfile()
    {

        if (Session["CameFromSocialNetwork"] == null) {
            var profile = ProfileBase.Create(tfEmail.Text);
            profile.SetPropertyValue("BirthDate", BirthdayField);
            profile.SetPropertyValue("Company", tfCompany.Text);
            profile.SetPropertyValue("CountryID", ddlCountry.SelectedValue);
            profile.SetPropertyValue("Email", tfEmail.Text);
            profile.SetPropertyValue("Name", tfName.Text);
            profile.SetPropertyValue("Phone", tfPhone.Text);
            profile.SetPropertyValue("SendNewsletter", this.chbReceiveNews.Checked);

            try {
                profile.Save();
                this.pnlSuceeded.Visible = true;
                this.pnlForm.Visible = false;
                if (EditMode && !OuterCreation) {
                    this.lblSuceeded.Text = Resources.Appleseed.USER_UPDATED_SUCCESFULLY;
                } else {
                    if (OuterCreation) {
                        this.lblSuceeded.Text = Resources.Appleseed.USER_UPDATED_SUCCESFULLY;
                        if (chbSendNotification.Checked) {
                            SendOuterCreationNotification(tfEmail.Text);
                        }
                    } else {
                        this.lblSuceeded.Text = Resources.Appleseed.USER_UPDATED_SUCCESFULLY + "." + Resources.Appleseed.ENTER_THROUGH_HOMEPAGE;
                    }
                }
            } catch (Exception exc) {
                this.lblError.Text = Resources.Appleseed.USER_SAVING_ERROR;
                ErrorHandler.Publish(LogLevel.Error, "Error al salvar un perfil", exc);
            }
        } else {
            string username = null;
            if(Session["TwitterUserName"] != null)
                username = (string)Session["TwitterUserName"];
            else if(Session["FacebookUserName"] != null)
                username = (string)Session["FacebookUserName"];
            var profile = ProfileBase.Create(username);
            profile.SetPropertyValue("BirthDate", BirthdayField);
            profile.SetPropertyValue("Company", tfCompany.Text);
            profile.SetPropertyValue("CountryID", ddlCountry.SelectedValue);
            profile.SetPropertyValue("Email", tfEmail.Text);
            profile.SetPropertyValue("Name", tfName.Text);
            profile.SetPropertyValue("Phone", tfPhone.Text);
            profile.SetPropertyValue("SendNewsletter", this.chbReceiveNews.Checked);
            try {
                profile.Save();

            } catch (Exception exc) {
                this.lblError.Text = Resources.Appleseed.USER_SAVING_ERROR;
                ErrorHandler.Publish(LogLevel.Error, "Error al salvar un perfil", exc);
            }
            if (Session["deleteCookies"] != null) {
                Session.Remove("CameFromSocialNetwork");
                Session.Remove("TwitterUserName");
                Session.Remove("deleteCookies");
            }
        }
    }


    private void BindCountry()
    {
        IList<Country> cs = GeographicProvider.Current.GetCountries(CountryFields.Name);
        this.ddlCountry.DataSource = cs;
        this.ddlCountry.DataBind();
    }


    private void SendOuterCreationNotification(string email)
    {
        //TODO
        string msg = Resources.Appleseed.NEW_USER_CREATED + this.PortalSettings.PortalName;
        msg += "<br/>" + Resources.Appleseed.USER + ": " + email;
        string pwd = Membership.GetUser(UserName).GetPassword();
        pwd = String.IsNullOrEmpty(pwd) ? "<vacío>" : pwd;
        msg += "<br/>" + Resources.Appleseed.PASSWORD + ": " + pwd;
        string uName = UserName;
        if (string.IsNullOrEmpty(uName))
        {
            uName = Convert.ToString(this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"]);
        }
        SendMail(email, email, "Nuevo usuario en " + this.PortalSettings.PortalName + "!", msg);
    }

    private void SendMail(string to, string from, string subject, string content)
    {
        //BulkMailService bMailService = new BulkMailService();
        //string[] toAddr = to.Split((';'));
        //string templateName="RegisterTemplate";
        //MailDTO mail = new MailDTO();
        //mail.ApplicationName = Membership.ApplicationName;
        //try {
        //    if (BulkMailProviderManager.Provider.ExistsTemplate(templateName)) {
        //        mail.Body = string.Empty;
        //    } else {
        //        mail.Body = content;
        //    }
        //} catch (Exception ex) {
        //    mail.Body = content;
        //} 


        //mail.CreatedBy = Membership.GetUser() != null ? Membership.GetUser().UserName : "anonymous";
        //mail.CreatedOn = DateTime.Now;
        //mail.Description = "Creación de Usuario";
        //mail.From = from;
        //mail.Priority = 0;
        //mail.Subject = subject;



        //List<MailParametersDTO> parameters = new List<MailParametersDTO>();
        //string path = Path.ApplicationFullPath;
        //if (Path.ApplicationRoot != null && Path.ApplicationRoot.Length > 0) {
        //    path = path.Replace(Path.ApplicationRoot, string.Empty);
        //}



        //MailParametersDTO logo = new MailParametersDTO();
        //logo.Key = "ImgLogo";

        //IUserServices userServices = AbtourServicesFactory.Current.UserServices;
        //try {

        //    Guid providerUserKey = PortalSettings.CurrentUser.Identity.ProviderUserKey;
        //    Abtour.Providers.MembershipProvider.AbtourUser membershipUser = Membership.GetUser(providerUserKey) as Abtour.Providers.MembershipProvider.AbtourUser;
        //    if (membershipUser != null) {
        //        AbtourUserEntity abtourUser = userServices.LoadAbtourUser(membershipUser.Id, true);
        //        // se esta asumiento que user siempre esta asociado a una agencia, por lo menos la agencia vacía.
        //        if (!(abtourUser.Agency.Fields["LogoUrl"].IsNull || abtourUser.Agency.LogoUrl.Equals(string.Empty))) {
        //            logo.Value = Path.ApplicationFullPath + "/" + abtourUser.Agency.LogoUrl;
        //        }
        //        MailParametersDTO bunner = new MailParametersDTO();
        //        bunner.Key = "HTMLBanner";

        //        if (abtourUser.Agency.Fields["HtmlBanner"].IsNull || abtourUser.Agency.HtmlBanner.Equals(string.Empty)) {
        //            bunner.Value = string.Empty;
        //        } else {
        //            bunner.Value = "<span>" + abtourUser.Agency.HtmlBanner + "</span>";
        //        }
        //        parameters.Add(bunner);


        //    } else {
        //        logo.Value = path + this.portalSettings.PortalFullPath + "/images/logo.gif";

        //        MailParametersDTO cabezal = new MailParametersDTO();
        //        cabezal.Key = "ImgCabezal";
        //        cabezal.Value = path + this.portalSettings.PortalFullPath + "/images/cabezal.jpg";
        //        parameters.Add(cabezal);
        //    }
        //} catch (Exception) {
        //    Agency agt = AbtourServicesFactory.Current.AgencyServices.LoadAgency(0);
        //    logo.Value = Path.ApplicationFullPath + "/" + agt.LogoUrl;
        //    MailParametersDTO bunner = new MailParametersDTO();
        //    bunner.Key = "HTMLBanner";
        //    bunner.Value = "<span>" + agt.HtmlBanner + "</span>";
        //    parameters.Add(bunner);
        //}

        //parameters.Add(logo);

        //MailParametersDTO websiteName = new MailParametersDTO();
        //websiteName.Key = "WebsiteName";
        //websiteName.Value = Path.ApplicationFullPath;
        //parameters.Add(websiteName);

        //mail.Template = templateName;

        //bMailService.SendMail(mail, to, templateName, true, parameters);
    }

    #endregion

    private string GeneratePasswordHash(string thisPassword)
    {
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