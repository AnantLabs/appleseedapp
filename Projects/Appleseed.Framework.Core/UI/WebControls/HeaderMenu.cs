using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Site.Data;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// HeaderMenu
    /// </summary>
    [History("jviladiu@portalServices.net", "2004/09/29", "Added link showHelp for show help window")]
    [
        History("ozan@Appleseed.web.tr", "2004/07/02",
            "Added  showTabMan and showTabAdd properties for managing tab and adding tab only one click... ")]
    [
        History("John.Mandia@whitelightsolutions.com", "2003/11/04",
            "Added extra property DataBindOnInit. So you can decide if you wish it to bind automatically or when you call DataBind()"
            )]
    [
        History("John.Mandia@whitelightsolutions.com", "2003/10/25",
            "Added ability to have more control over the menu by adding more settings.")]
    public class HeaderMenu : DataList, INamingContainer
    {
        private object innerDataSource = null;

        private bool _showLogon = false;
        private bool _dialogLogon = false;
        private string _dialogLogonControlPath = "~/DesktopModules/CoreModules/SignIn/SignIn.ascx";
        private bool _showSecureLogon = false; // Thierry (Tiptopweb), 5 May 2003: add link to Secure directory
        private bool _showHome = true;
        private bool _showTabMan = true; // Ozan, 2 June 2004: add link for tab management 
        private bool _showRegister = false;
        private bool _showDragNDrop = false;

        // 26 October 2003 john.mandia@whitelightsolutions.com - Start
        private bool _showEditProfile = true;
        private bool _showWelcome = true;
        private bool _showLogOff = true;
        private bool _dataBindOnInit = true;
        
        // 26 October 2003 John Mandia - Finish

        private bool _showHelp = false; // Jos� Viladiu, 29 Sep 2004: Add link for show help window

        /// <summary>
        /// If true shows a link to a Help Window
        /// </summary>
        /// <value><c>true</c> if [show help]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowHelp
        {
            get { return _showHelp; }
            set { _showHelp = value; }
        }

        /// <summary>
        /// If true and user is not authenticated shows
        /// a logon link in place of logoff
        /// </summary>
        /// <value><c>true</c> if [show logon]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowLogon
        {
            get { return _showLogon; }
            set { _showLogon = value; }
        }

        /// <summary>
        /// If true and ShowLogon is also true, when the user clicks the logon link
        /// a dialog will be displayed for the user to logon.
        /// </summary>
        /// <value><c>true</c> if [dialog logon]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool DialogLogon
        {
            get { return _dialogLogon; }
            set { _dialogLogon = value; }
        }

        /// <summary>
        /// The path of the ascx control that will be displayed inside the dialog for the user to sign in.
        /// The default is "~/DesktopModules/CoreModules/SignIn/SignIn.ascx".
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue("~/DesktopModules/CoreModules/SignIn/SignIn.ascx")
            ]
        public string DialogLogonControlPath
        {
            get { return _dialogLogonControlPath; }
            set { _dialogLogonControlPath = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowRegister
        {
            get { return _showRegister; }
            set { _showRegister = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowDragNDrop
        {
            get { return _showDragNDrop; }
            set { _showDragNDrop = value; }
        }


        /// <summary>
        /// If true and user is not authenticated shows
        /// a SECURE logon link in place of logoff
        /// </summary>
        /// <value><c>true</c> if [show secure logon]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowSecureLogon
        {
            get { return _showSecureLogon; }
            set { _showSecureLogon = value; }
        }

        /// <summary>
        /// Whether show home link
        /// </summary>
        /// <value><c>true</c> if [show home]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool ShowHome
        {
            get { return _showHome; }
            set { _showHome = value; }
        }

        // 2 June 2004 Ozan
        /// <summary>
        /// Whether show Manage Tab link
        /// </summary>
        /// <value><c>true</c> if [show tab man]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowTabMan
        {
            get { return _showTabMan; }
            set { _showTabMan = value; }
        }

        // 26 October 2003 john.mandia@whitelightsolutions.com - Start
        /// <summary>
        /// Whether Edit Profile link
        /// </summary>
        /// <value><c>true</c> if [show edit profile]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool ShowEditProfile
        {
            get { return _showEditProfile; }
            set { _showEditProfile = value; }
        }

        /// <summary>
        /// Whether Welcome Shows
        /// </summary>
        /// <value><c>true</c> if [show welcome]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool ShowWelcome
        {
            get { return _showWelcome; }
            set { _showWelcome = value; }
        }

        /// <summary>
        /// Whether Logoff Link Shows
        /// </summary>
        /// <value><c>true</c> if [show log off]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool ShowLogOff
        {
            get { return _showLogOff; }
            set { _showLogOff = value; }
        }

        /// <summary>
        /// Whether Logoff Link Shows
        /// </summary>
        /// <value><c>true</c> if [data bind on init]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool DataBindOnInit
        {
            get { return _dataBindOnInit; }
            set { _dataBindOnInit = value; }
        }

        // 26 October 2003 John Mandia - Finish

        /// <summary>
        /// HeaderMenu
        /// </summary>
        public HeaderMenu()
        {
            ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
            RepeatDirection = RepeatDirection.Horizontal;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event for the <see cref="T:System.Web.UI.WebControls.DataList"></see> control.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (DataBindOnInit)
            {
                DataBind();
            }
        }

        // Jes1111
        /// <summary>
        /// Builds a help link for the header menu and registers it with the page
        /// </summary>
        /// <returns></returns>
        private string GetHelpLink()
        {


            // Jes1111 - 27/Nov/2004 - simplified help popup scheme (echoes changes in ModuleButton.cs)
            string helpTarget = "AppleseedHelp";
            string popupOptions =
                "toolbar=1,location=0,directories=0,status=0,menubar=1,scrollbars=1,resizable=1,width=600,height=400,screenX=15,screenY=15,top=15,left=15";
            string helpText = General.GetString("HEADER_HELP", "Help");

            StringBuilder sb = new StringBuilder();
            sb.Append("<a href=\"");
            sb.Append(Path.ApplicationRoot);
            sb.Append("/rb_documentation/Viewer.aspx\"	target=\"");
            sb.Append(helpTarget);
            sb.Append("\" ");
            if (Page is Page)
            {
                sb.Append("onclick=\"link_popup(this,'");
                sb.Append(popupOptions);
                sb.Append("');return false;\"");
            }
            sb.Append(" class=\"");
            sb.Append("link-is-popup");
            if (CssClass.Length != 0)
            {
                sb.Append(" ");
                sb.Append(CssClass);
            }
            sb.Append("\">");
            sb.Append(helpText);
            sb.Append("</a>");

            if (Page is Page)
            {
                if (!((Page) Page).ClientScript.IsClientScriptIncludeRegistered("rb-popup"))
                    ((Page) Page).ClientScript.RegisterClientScriptInclude(((Page) Page).GetType(), "rb-popup",
                                                                           Path.ApplicationRoot +
                                                                           "/aspnet_client/popupHelper/popup.js");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Binds the control and all its child controls to the specified data source.
        /// </summary>
        public override void DataBind()
        {
            if (HttpContext.Current != null)
            {
                //Init data
                ArrayList list = new ArrayList();

                // Obtain PortalSettings from Current Context
                PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

                string homeLink = "<a";
                string menuLink;

                // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                if (CssClass.Length != 0)
                    homeLink = homeLink + " class=\"" + CssClass + "\"";

                homeLink = homeLink + " href='" + HttpUrlBuilder.BuildUrl() + "'>" +
                           General.GetString("Appleseed", "HOME") + "</a>";

                // If user logged in, customize welcome message
                if (HttpContext.Current.Request.IsAuthenticated == true)
                {
                    if (ShowWelcome)
                    {
                        list.Add(General.GetString("HEADER_WELCOME", "Welcome", this) + "&#160;" +
                                 PortalSettings.CurrentUser.Identity.Name + "!");
                    }

                    if (ShowHome)
                    {
                        list.Add(homeLink);
                    }

                    if (ShowHelp)
                    {
                        list.Add(GetHelpLink());
                    }

                    // Added by Mario Endara <mario@softworks.com.uy> (2004/11/06)
                    // Find Tab module to see if the user has add/edit rights
                    ModulesDB modules = new ModulesDB();
                    Guid TabGuid = new Guid("{1C575D94-70FC-4A83-80C3-2087F726CBB3}");
                    // Added by Xu Yiming <ymhsu@ms2.hinet.net> (2004/12/6)
                    // Modify for support Multi or zero Pages Modules in a single portal.
                    bool HasEditPermissionsOnTabs = false;
                    int TabModuleID = 0;

//					SqlDataReader result = modules.FindModulesByGuid(PortalSettings.PortalID, TabGuid);
//					while(result.Read()) 
//					{
//						TabModuleID=(int)result["ModuleId"];

                    foreach (ModuleItem m in modules.FindModuleItemsByGuid(PortalSettings.PortalID, TabGuid))
                    {
                        HasEditPermissionsOnTabs = PortalSecurity.HasEditPermissions(m.ID);
                        if (HasEditPermissionsOnTabs)
                        {
                            TabModuleID = m.ID;
                            break;
                        }
                    }

                    // If user logged in and has Edit permission in the Tab module, reach tab management just one click
                    if ((ShowTabMan) && (HasEditPermissionsOnTabs))
                    {
                        // added by Mario Endara 2004/08/06 so PageLayout can return to this page
                        // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        // added mID by Mario Endara <mario@softworks.com.uy> to support security check (2004/11/09)
                        menuLink = menuLink + " href='" +
                                   HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Pages/PageLayout.aspx?PageID=") +
                                   PortalSettings.ActivePage.PageID + "&amp;mID=" + TabModuleID.ToString() +
                                   "&amp;Alias=" + PortalSettings.PortalAlias + "&amp;lang=" + PortalSettings.PortalUILanguage +
                                   "&amp;returntabid=" + PortalSettings.ActivePage.PageID + "'>" +
                                   General.GetString("HEADER_MANAGE_TAB", "Edit This Page", null) + "</a>";
                        list.Add(menuLink);
                    }

                    if (ShowDragNDrop && HasEditPermissionsOnTabs) {

                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        menuLink = menuLink + " href='javascript:DnD();'>" + General.GetString("DRAGNDROP", "DragNDrop", null) + "</a>";
                        list.Add(menuLink);
                    }

                    if (ShowEditProfile)
                    {
                        // 19/08/2004 Jonathan Fong
                        // www.gt.com.au
                        if ( Context.User.Identity.AuthenticationType == "LDAP" ) {
                            // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                            menuLink = "<a";
                            if ( CssClass.Length != 0 )
                                menuLink = menuLink + " class=\"" + CssClass + "\"";

                            menuLink = menuLink + " href='" +
                                       HttpUrlBuilder.BuildUrl( "~/DesktopModules/CoreModules/Register/Register.aspx", "userName=" +
                                                                                                          PortalSettings
                                                                                                              .CurrentUser
                                                                                                              .Identity.
                                                                                                                UserName) +
                                       "'>" + "Profile" + "</a>";
                            list.Add( menuLink );
                        }
                        // If user is form add edit user link
                        else if ( !( HttpContext.Current.User is WindowsPrincipal ) ) {
                            // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                            menuLink = "<a";
                            if ( CssClass.Length != 0 )
                                menuLink = menuLink + " class=\"" + CssClass + "\"";

                            menuLink = menuLink + " href='" +
                                       HttpUrlBuilder.BuildUrl( "~/DesktopModules/CoreModules/Register/Register.aspx", "userName=" +
                                                                                                          PortalSettings
                                                                                                              .
                                                                                                              CurrentUser
                                                                                                              .Identity.
                                                                                                              UserName) +
                                       "'>" +
                                       General.GetString( "HEADER_EDIT_PROFILE", "Edit profile", this ) + "</a>";
                            list.Add( menuLink );
                        }
                    }

                    // if authentication mode is Cookie, provide a logoff link
                    if (Context.User.Identity.AuthenticationType == "Forms" ||
                        Context.User.Identity.AuthenticationType == "LDAP")
                    {
                        if (ShowLogOff)
                        {
                            // Corrections when ShowSecureLogon is true. jviladiu@portalServices.net (05/07/2004)
                            string href = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/Logoff.aspx");
                            if (ShowSecureLogon && Context.Request.IsSecureConnection)
                            {
                                string auxref = Context.Request.Url.AbsoluteUri;
                                auxref = auxref.Substring(0, auxref.IndexOf(Context.Request.Url.PathAndQuery));
                                href = auxref + href;
                                href = href.Replace("https", "http");
                            }
                            // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                            menuLink = "<a";
                            if (CssClass.Length != 0)
                                menuLink = menuLink + " class=\"" + CssClass + "\"";

                            menuLink = menuLink + " href='" + href + "'>" +
                                       General.GetString("HEADER_LOGOFF", "Logoff", null) + "</a>";
                            list.Add(menuLink);
                        }
                    }
                }
                else
                {
                    if (ShowHome)
                    {
                        list.Add(homeLink);
                    }

                    if (ShowHelp)
                    {
                        list.Add(GetHelpLink());
                    }

                    // if not authenticated and ShowLogon is true, provide a logon link 

                    if (ShowLogon)
                    {
                        // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        menuLink += string.Concat(" id=\"", this.ClientID, "_logon_link" , "\"");
                        menuLink = menuLink + " href='" + HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/Logon.aspx") +
                                   "'>" + General.GetString("LOGON", "Logon", null) + "</a>";
                        list.Add(menuLink);
                    }

                    var allowNewRegistration = false;
                    if (PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"] != null)
                        if (bool.Parse(PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"].ToString()))
                            allowNewRegistration = true;

                    if (ShowRegister && allowNewRegistration) {
                        
                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        menuLink = menuLink + " href='" + HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx") +
                                   "'>" + General.GetString("REGISTER", "Register", null) + "</a>";
                        list.Add(menuLink);
                    }



                    // Thierry (Tiptopweb) 5 May 2003 : Secure Logon to Secure Directory
                    if (ShowSecureLogon)
                    {
                        // Added localized support. jviladiu@portalServices.net (05/07/2004)
                        // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        menuLink = menuLink + " href='" + PortalSettings.PortalSecurePath + "/Logon.aspx'>" +
                                   General.GetString("LOGON", "Logon", null) + "</a>";
                        list.Add(menuLink);
                    }
                }
                innerDataSource = list;
            }
            base.DataBind();
            if (ShowLogon && DialogLogon)
            {
                //this new list control won't appear in the list, since it has no DataItem. However we need it for "holding" the Signin Control.
                var newItem = new DataListItem(this.Controls.Count, ListItemType.Item);
                this.Controls.Add(newItem);
                
                var logonDialogPlaceHolder = new PlaceHolder();
                newItem.Controls.Add(logonDialogPlaceHolder);

                if (_logonControl == null) //we ask this in case someone call the Databind more than once.
                {
                    _logonControl = Page.LoadControl(DialogLogonControlPath);
                    _logonControl.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                }
                logonDialogPlaceHolder.Controls.Add(_logonControl);
            }
        }

        //we added this private attribute for store a reference to the signin control. 
        //Its assigned during the binding (and the con
        private Control _logonControl = null;

        /// <summary>
        /// DataSource
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Collections.IEnumerable"></see> or <see cref="T:System.ComponentModel.IListSource"></see> that contains a collection of values used to supply data to this control. The default value is null.</returns>
        /// <exception cref="T:System.Web.HttpException">The data source cannot be resolved because a value is specified for both the <see cref="P:System.Web.UI.WebControls.BaseDataList.DataSource"></see> property and the <see cref="P:System.Web.UI.WebControls.BaseDataList.DataSourceID"></see> property. </exception>
        public override object DataSource
        {
            get { return innerDataSource; }
            set { innerDataSource = value; }
        }


        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that contains the output stream to render on the client.</param>
        /// <remarks></remarks>
        protected override void Render(HtmlTextWriter writer)
        {
            if (ShowLogon && DialogLogon && _logonControl != null)
            {
                writer.Write(string.Concat("<div id=\"", this.ClientID ,"_logon_dialog\" style=\"display:none\">"));
                _logonControl.RenderControl(writer);
                writer.Write("</div>");
                
                writer.Write("<script type=\"text/javascript\">");

                writer.Write(string.Concat(@"
                        $(document).ready(function () {
                            var $dialog = $('#", this.ClientID, @"_logon_dialog')
                            .dialog({
                                autoOpen: false,
                                modal: true,
                                resizable: false,
                                open: function (type, data) { $(this).parent().appendTo('form'); }
                            });

                            $('#", this.ClientID, @"_logon_link').click(function () {
                                $dialog.dialog('open');
                                // prevent the default action, e.g., following a link
                                return false;
                            });
                            //this is a hack, we should find another way to know if the dialog should autoopen.
                            if ($('#", this.ClientID, @"_logon_dialog span.Error').html()) {
                                $dialog.dialog('open');
                            }
                        });"
                    )); 
                
                writer.Write("</script>");
            }
            base.Render(writer);
        }
    }
}
