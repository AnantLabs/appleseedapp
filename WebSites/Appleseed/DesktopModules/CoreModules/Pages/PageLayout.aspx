<%@ Page AutoEventWireup="false" Inherits="Appleseed.Admin.PageLayout" Language="c#" 
MasterPageFile="~/Shared/SiteMasterDefault.master" Codebehind="PageLayout.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <table border="0" cellpadding="2" cellspacing="1" class="ModuleWrap" style="height:100%">
        <tr>
            <td>
            <div id="accordion">
                    <h3><a href="#">Page Information</a></h3>
                    <div id="Page_Information" style="height: 331px;">
                        <% if (Request.QueryString.GetValues("ModalChangeMaster") == null) { %>
                        <table border="0" cellpadding="2" cellspacing="1" class="ModuleWrap">
                            <tr>
                                <td colspan="4">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" class="Head">
                                                <rbfwebui:Localize ID="tab_name" runat="server" Text="Page Layouts" TextKey="AM_TABNAME" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr noshade="noshade" size="1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <% } %>
                        <table border="0" cellpadding="2" cellspacing="1" class="ModuleWrap" style="height: 100%">
                            <tr>
                            <td class="Normal" width="100">
                                <rbfwebui:Localize ID="tab_name1" runat="server" Text="Page Name" TextKey="AM_TABNAME1">
                                </rbfwebui:Localize>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="tabName" runat="server" CssClass="NormalTextBox" MaxLength="50"
                                    Width="300" OnTextChanged="PageSettings_Change" />
                            </td>
                            </tr>
                            <tr>
                                <td class="Normal" nowrap="nowrap">
                                    <rbfwebui:Localize ID="roles_auth" runat="server" Text="Authorized Roles" TextKey="AM_ROLESAUTH">
                                    </rbfwebui:Localize>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBoxList ID="authRoles" runat="server" CssClass="Normal" RepeatColumns="2"
                                        Width="300" OnSelectedIndexChanged="PageSettings_Change" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Normal" nowrap="nowrap">
                                    <rbfwebui:Localize ID="tab_parent" runat="server" Text="Parent Page" TextKey="TAB_PARENT">
                                    </rbfwebui:Localize>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="parentPage" runat="server" CssClass="NormalTextBox" Width="300px"
                                        DataTextField="Name" DataValueField="ID">
                                    </asp:DropDownList>
                                    <rbfwebui:Label ID="lblErrorNotAllowed" runat="server" CssClass="Error" EnableViewState="False"
                                        TextKey="ERROR_NOT_ALLOWED_PARENT" Visible="False">Not allowed to choose that parent</rbfwebui:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <hr noshade="noshade" size="1" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Normal" nowrap="nowrap">
                                    <rbfwebui:Localize ID="show_mobile" runat="server" Text="Show to mobile users" TextKey="AM_SHOWMOBILE">
                                    </rbfwebui:Localize>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBox ID="showMobile" runat="server" CssClass="Normal" OnCheckedChanged="PageSettings_Change" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Normal" nowrap="nowrap">
                                    <rbfwebui:Localize ID="mobiletab" runat="server" Text="Mobile Page Name" TextKey="AM_MOBILETAB">
                                    </rbfwebui:Localize>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="mobilePageName" runat="server" CssClass="NormalTextBox" Width="300"
                                        OnTextChanged="PageSettings_Change" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <hr noshade="noshade" size="1" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h3><a href="#">Page Modules</a></h3>
                    <div id="Page_Modules">
                        <table border="0" cellpadding="2" cellspacing="1" class="ModuleWrap">  
                            <tr>
                                <td class="Normal">
                                    <rbfwebui:Localize ID="addmodule" runat="server" Text="Add module" TextKey="AM_ADDMODULE">
                                    </rbfwebui:Localize>
                                </td>
                                <td class="Normal">
                                    <rbfwebui:Localize ID="module_type" runat="server" Text="Module type" TextKey="AM_MODULETYPE">
                                    </rbfwebui:Localize>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="moduleType" runat="server" CssClass="NormalTextBox" DataTextField="key"
                                        DataValueField="value">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="Normal">
                                    <rbfwebui:Localize ID="moduleLocationLabel" runat="server" Text="Module Location:"
                                        TextKey="AM_MODULELOCATION">
                                    </rbfwebui:Localize>
                                </td>
                                <td colspan="2" valign="top">
                                    <asp:DropDownList ID="paneLocation" runat="server">
                                        <asp:listitem value="TopPane">Header</asp:listitem>
                                        <asp:listitem value="LeftPane">Left Column</asp:listitem>
                                        <asp:listitem selected="True" value="ContentPane">Center Column</asp:listitem>
                                        <asp:listitem value="RightPane">Right Column</asp:listitem>
                                        <asp:listitem value="BottomPane">Footer</asp:listitem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="Normal" valign="top">
                                    <rbfwebui:Localize ID="moduleVisibleLabel" runat="server" Text="Module Visible To:"
                                        TextKey="AM_MODULEVISIBLETO">
                                    </rbfwebui:Localize>
                                </td>
                                <td colspan="2" valign="top">
                                    <asp:DropDownList ID="viewPermissions" runat="server">
                                        <asp:ListItem Selected="True" Value="All Users;">All Users</asp:ListItem>
                                        <asp:ListItem Value="Authenticated Users;">Authenticated Users</asp:ListItem>
                                        <asp:ListItem Value="Unauthenticated Users;">Unauthenticated Users</asp:ListItem>
                                        <asp:ListItem Value="Admins;">Admins Role</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="Normal">
                                    <rbfwebui:Localize ID="module_name" runat="server" Text="Module Name" TextKey="AM_MODULENAME">
                                    </rbfwebui:Localize>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="moduleTitle" runat="server" CssClass="NormalTextBox" EnableViewState="false"
                                        Text="New Module Name" Width="250"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <a id="Content_AddModuleBtn" class="CommandButton" href="#" onclick="addModule('<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/AddModule") %>');return false;">Add to "Organize Modules" Below</a>
                                    <asp:HiddenField ID="PageIdField" runat="server" />
                                    <asp:HiddenField ID="ModuleIdField" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <hr noshade="noshade" size="1" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="Normal" rowspan="3">
                                    <rbfwebui:Localize ID="organizemodule" runat="server" Text="Organize Module" TextKey="AM_ORGANIZEMODULE">
                                    </rbfwebui:Localize>
                                </td>
                                <td width="*" colspan="3">
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="NormalBold">
                                                &nbsp;
                                                <rbfwebui:Localize ID="topPanel" runat="server" Text="Top Pane" TextKey="AM_TOPPANEL">
                                                </rbfwebui:Localize>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table border="0" cellpadding="0" cellspacing="2">
                                                    <tr valign="top">
                                                        <td rowspan="2">
                                                            <select id="Content_topPane" class="NormalTextBox" style="width: 690px" size="8"></select>                                                            
                                                        </td>
                                                        <td nowrap="nowrap" valign="top">
                                                            <input id="Content_TopUpBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Up", "Up.gif").ImageUrl %>"
                                                             alt="Move selected item up in list"  onclick="mvUpDown('up', 'TopPane','Content_topPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_TopDownBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Down", "Down.gif").ImageUrl %>"
                                                             alt="Move selected item down in list"  onclick="mvUpDown('down', 'TopPane','Content_topPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                            <br />
                                                             <input id="Content_TopRightBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Bottom", "Right.gif").ImageUrl %>"
                                                             alt="Move selected item to Content"  onclick="mvRigthLeft('TopPane', 'ContentPane', 'Content_topPane', '<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>');return false;"/>&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" valign="Top">
                                                            <input id="Content_TopEditBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>"
                                                             alt="Edit"  onclick="editModule('TopPane', 'Content_topPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/EditBtn_Click") %>');return false;"/>
                                                            <br />
                                                            <br />
                                                            <input id="Content_TopDeleteBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>"
                                                             alt="Delete"  onclick="return deleteModule('TopPane', 'Content_topPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/DeleteBtn_Click") %>');"/>
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                           
                            <ContentTemplate>
                                <tr valign="top">
                                <td width="120">
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="NormalBold">
                                                <rbfwebui:Localize ID="LeftPanel" runat="server" Text="Left Pane" TextKey="AM_LEFTPANEL">
                                                </rbfwebui:Localize>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="2">
                                                    <tr valign="top">
                                                        <td rowspan="2">
                                                            <select id="Content_leftPane" class="NormalTextBox" style="width: 110px" size="8"></select> 
                                                        </td>
                                                        <td nowrap="nowrap" valign="top">
                                                            <input id="Content_LeftTopBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Top", "Left.gif").ImageUrl %>"
                                                             alt="Move selected item to the header"  onclick="mvRigthLeft('LeftPane', 'TopPane', 'Content_leftPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>    ');return false;"/>
                                                            <br/>
                                                            <input id="Content_LeftUpBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Up", "Up.gif").ImageUrl %>"
                                                             alt="Move selected item up in list"  onclick="mvUpDown('up', 'LeftPane', 'Content_leftPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_LeftRightBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Right", "Right.gif").ImageUrl %>"
                                                             alt="Move selected item To Content pane"  onclick="mvRigthLeft('LeftPane', 'ContentPane', 'Content_leftPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>');return false;"/>
                                                            <br />
                                                             <input id="Content_LeftDownBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Down", "Down.gif").ImageUrl %>"
                                                             alt="Move selected item down in list"  onclick="mvUpDown('down', 'LeftPane', 'Content_leftPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                            <br/>
                                                            <input id="Content_LeftBottomBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Bottom", "Right.gif").ImageUrl %>"
                                                             alt="Move selected item to Bottom pane"  onclick="mvRigthLeft('LeftPane', 'BottomPane', 'Content_leftPane', '<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>    ');return false;"/>&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" valign="bottom">
                                                            <input id="Content_LeftEditBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>"
                                                             alt="Edit"  onclick="editModule('LeftPane', 'Content_leftPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/EditBtn_Click") %>');return false;" />
                                                            <br />
                                                            <br />
                                                            <input id="Content_LeftDeleteBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>"
                                                             alt="Delete"  onclick="return deleteModule('LeftPane', 'Content_leftPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/DeleteBtn_Click") %>');"/>
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td >
                                    <table border="0" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td class="NormalBold">
                                                &nbsp;
                                                <rbfwebui:Localize ID="contentpanel" runat="server" Text="Content Pane" TextKey="AM_CENTERPANEL">
                                                </rbfwebui:Localize>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table border="0" cellpadding="0" cellspacing="2" width="100%">
                                                    <tr valign="top">
                                                        <td rowspan="2">
                                                            <select id="Content_contentPane" class="NormalTextBox" style="width: 290px" size="8"></select> 
                                                        </td>
                                                        <td nowrap="nowrap" valign="top">
                                                            <input id="Content_ContentTopBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Top", "Left.gif").ImageUrl %>"
                                                             alt="Move selected item to the header"  onclick="mvRigthLeft('ContentPane', 'TopPane', 'Content_contentPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_ContentUpBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Up", "Up.gif").ImageUrl %>"
                                                             alt="Move selected item up in list"  onclick="mvUpDown('up', 'ContentPane', 'Content_contentPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                            <br />
                                                             <input id="Content_ContentLeftBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Left", "Left.gif").ImageUrl %>"
                                                             alt="Move selected item to the left pane"  onclick="mvRigthLeft('ContentPane', 'LeftPane', 'Content_contentPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_ContentRightBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Right", "Right.gif").ImageUrl %>"
                                                             alt="Move selected item To Right pane"  onclick="mvRigthLeft('ContentPane', 'RightPane', 'Content_contentPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_ContentDownBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Down", "Down.gif").ImageUrl %>"
                                                             alt="Move selected item down in list"  onclick="mvUpDown('down', 'ContentPane', 'Content_contentPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                             <br />
                                                            <input id="Content_ContentBottomBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Bottom", "Right.gif").ImageUrl %>"
                                                             alt="Move selected item to Bottom pane"  onclick="mvRigthLeft('ContentPane', 'BottomPane', 'Content_contentPane', '<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>');return false;"/>&nbsp;&nbsp;
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" valign="bottom">
                                                            <input id="Content_ContentEditBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>"
                                                             alt="Edit"  onclick="editModule('ContentPane', 'Content_contentPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/EditBtn_Click") %>');return false;" />
                                                            <br />
                                                            <br />
                                                            <input id="Content_ContentDeleteBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>"
                                                             alt="Delete"  onclick="return deleteModule('ContentPane', 'Content_contentPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/DeleteBtn_Click") %>');"/>
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="120">
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="NormalBold">
                                                &nbsp;
                                                <rbfwebui:Localize ID="rightpanel" runat="server" Text="Right Pane" TextKey="AM_RIGHTPANEL">
                                                </rbfwebui:Localize>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="2">
                                                    <tr valign="top">
                                                        <td rowspan="2">
                                                             <select id="Content_rightPane" class="NormalTextBox" style="width: 110px" size="8"></select>
                                                        </td>
                                                        <td nowrap="nowrap" valign="top">
                                                            <input id="Content_RightTopBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Top", "Left.gif").ImageUrl %>"
                                                             alt="Move selected item to the header"  onclick="mvRigthLeft('RightPane', 'TopPane', 'Content_rightPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>    ');return false;"/>
                                                            <br />
                                                            <input id="Content_RightUpBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Up", "Up.gif").ImageUrl %>"
                                                             alt="Move selected item up in list"  onclick="mvUpDown('up', 'RightPane', 'Content_rightPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_RightLeftBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Left", "Left.gif").ImageUrl %>"
                                                             alt="Move selected item to the content pane"  onclick="mvRigthLeft('RightPane', 'ContentPane', 'Content_rightPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_RightDownBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Down", "Down.gif").ImageUrl %>"
                                                             alt="Move selected item down in list"  onclick="mvUpDown('down', 'RightPane', 'Content_rightPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_RightBottomBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Bottom", "Right.gif").ImageUrl %>"
                                                             alt="Move selected item to Bottom pane"  onclick="mvRigthLeft('RightPane', 'BottomPane', 'Content_rightPane', '<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>    ');return false;"/>&nbsp;&nbsp;
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" valign="bottom">
                                                            <input id="Content_RightEditBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>"
                                                             alt="Edit"  onclick="editModule('RightPane', 'Content_rightPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/EditBtn_Click") %>');return false;" />
                                                            <br />
                                                            <br />
                                                            <input id="Content_RightDeleteBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>"
                                                             alt="Delete"  onclick="return deleteModule('RightPane', 'Content_rightPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/DeleteBtn_Click") %>');"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            </ContentTemplate>
                            
                            <tr valign="top">
                                <td width="*" colspan="3">
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="NormalBold">
                                                &nbsp;
                                                <rbfwebui:Localize ID="bottomPanel" runat="server" Text="Bottom Pane" TextKey="AM_BOTOMPANEL">
                                                </rbfwebui:Localize>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table border="0" cellpadding="0" cellspacing="2">
                                                    <tr valign="top">
                                                        <td rowspan="2">
                                                            <select id="Content_bottomPane" class="NormalTextBox" style="width: 690px" size="8"></select>
                                                        </td>
                                                        <td nowrap="nowrap" valign="top">
                                                            <input id="Content_BottomLeftBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Top", "Left.gif").ImageUrl %>"
                                                             alt="Move selected item to the content pane"  onclick="mvRigthLeft('BottomPane', 'ContentPane', 'Content_bottomPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/RightLeft_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_BottomUpBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Up", "Up.gif").ImageUrl %>"
                                                             alt="Move selected item up in list"  onclick="mvUpDown('up', 'BottomPane', 'Content_bottomPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>
                                                            <br />
                                                            <input id="Content_BottomDownBtn" type="image" src="<%= CurrentTheme.GetImage("Buttons_Down", "Down.gif").ImageUrl %>"
                                                             alt="Move selected item down in list"  onclick="mvUpDown('down', 'BottomPane', 'Content_bottomPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/UpDown_Click") %>');return false;"/>&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" valign="bottom">
                                                            <input id="Image1" type="image" src="<%= CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl %>"
                                                             alt="Edit"  onclick="editModule('BottomPane', 'Content_bottomPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/EditBtn_Click") %>');return false;" />
                                                            <br />
                                                            <br />
                                                            <input id="Image2" type="image" src="<%= CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl %>"
                                                             alt="Delete"  onclick="return deleteModule('BottomPane', 'Content_bottomPane','<%= HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/DeleteBtn_Click") %>');"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
		                    </tr>
                            <tr>
                                <td align="center" class="Error" colspan="4">
                                    <rbfwebui:Localize ID="msgError" runat="server" Text="You do not have the appropriate permissions to delete or move this module"
                                        TextKey="AM_NO_RIGHTS">
                                    </rbfwebui:Localize>
                                </td>
                            </tr>
                         </table>
                    </div>
                    <h3><a href="#">Page Settings</a></h3>
                    <div id="Page Settings">
                        <table border="0" cellpadding="2" cellspacing="1" class="ModuleWrap">       
                            <tr>
                                <td colspan="4">
                                    <hr noshade="noshade" size="1" />
                                    <rbfwebui:SettingsTable ID="EditTable" runat="server" OnUpdateControl="EditTable_UpdateControl" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" cellpadding="2" cellspacing="1" class="ModuleWrap">    
                    <tr>
                        <td colspan="4">
                            <rbfwebui:LinkButton ID="UpdateButton" runat="server" class="CommandButton" Text="Apply Changes"
                                TextKey="APPLY_CHANGES"></rbfwebui:LinkButton>&nbsp;
                            <rbfwebui:LinkButton ID="CancelButton" runat="server" class="CommandButton" Text="Cancel"
                                TextKey="CANCEL"></rbfwebui:LinkButton>
                        </td>
                        </tr>
                </table>               
             </td>
        </tr>
    </table>
    
     
        <script type="text/javascript">
            $(function () {
                $("#accordion").accordion({
                    autoHeight: false,
                    navigation: true,
                    collapsible: true,
                    heightStyle: "fill"
                });
            });

            $(document).ready(function () {
                var page = $('#Content_PageIdField').val();
                FillPane("TopPane", page);
                FillPane("LeftPane", page);
                FillPane("ContentPane", page);
                FillPane("RightPane", page);
                FillPane("BottomPane", page);

            });

            function FillPane(panes, page) {
                
                $.ajax({
                    url: <%= urlToLoadModules %>,
                    type: "POST",
                    timeout: 180000,
                    data:{
                        pane: panes,
                        pageId: page
                    },
                    success: function (data) {
                        if (data.error == false) {
                            RewriteSelect(panes, data.value);
                        }
                    }
                });

            };

            function addModule(urltarget) {
                var titleM = $('#Content_moduleTitle').val();
                var type = $('#Content_moduleType').val();
                var pane = $('#Content_paneLocation').val();
                var perms = $('#Content_viewPermissions').val();
                var page = $('#Content_PageIdField').val();
                var module = $('#Content_ModuleIdField').val();

                $.ajax({
                    url: urltarget,
                    type: "POST",
                    timeout: 180000,
                    data: {
                        title: titleM,
                        moduleType: type,
                        paneLocation: pane,
                        viewPermission: perms,
                        pageId: page,
                        ModuleId: module
                    },
                    success: function (data) {
                        RewriteSelect(pane, data.value);
                        
                    }
                });
            };


            function mvUpDown(upordown,panes,paneId,urltarget) {

                var index = $('#'+paneId+' :selected').index();
                if (index != -1 && index > -1) {
                    var page = $('#Content_PageIdField').val();

                    var selectlenght = $('#'+paneId+' option').length;


                    $.ajax({
                        url: urltarget,
                        type: "POST",
                        timeout: 180000,
                        data: {
                            cmd: upordown,
                            pane: panes,
                            pageId: page,
                            selectedIndex: index,
                            length: selectlenght
                        },
                        success: function (data) {
                            if(!(data.value == 'error'))
                                RewriteSelect(panes, data.value);
                        }
                    });

                }

            }

            function RewriteSelect(pane, value){
                if (pane == 'LeftPane') {
                    $('#Content_leftPane').html("");
                    $('#Content_leftPane').html(value);
                }
                else if (pane == 'TopPane') {
                    $('#Content_topPane').html("");
                    $('#Content_topPane').html(value);
                }
                else if (pane == 'ContentPane') {
                    $('#Content_contentPane').html("");
                    $('#Content_contentPane').html(value);
                }
                else if (pane == 'RightPane') {
                    $('#Content_rightPane').html("");
                    $('#Content_rightPane').html(value);
                }
                else {
                    $('#Content_bottomPane').html("");
                    $('#Content_bottomPane').html(value);
                }

            }

            

            function mvRigthLeft(source, target, paneId,urlTarget) {
                var index = $('#' + paneId + ' :selected').index();
                if (index != -1 && index > -1) {
                    var page = $('#Content_PageIdField').val();

                    $.ajax({
                        url: urlTarget,
                        type: "POST",
                        timeout: 180000,
                        data: {
                            sourcePane: source,
                            targetPane: target,
                            pageId: page,
                            selectedIndex: index
                        },
                        success: function (data) {
                            if (data.error == false) {
                                RewriteSelect(source, data.source);
                                RewriteSelect(target, data.target);
                            }

                        }
                    });
                
                }
            }



            function deleteModule(paneLocation, paneId,urlTarget) {
                var agree = confirm("Delete this item?");
                if (agree) {
                    var index = $('#' + paneId + ' :selected').index();
                    if (index != -1 && index > -1) {
                        var page = $('#Content_PageIdField').val();

                        $.ajax({
                            url: urlTarget,
                            type: "POST",
                            timeout: 180000,
                            data: {
                                pane: paneLocation,
                                pageId: page,
                                selectedIndex: index
                            },
                            success: function (data) {
                                if (data.error == false)
                                    RewriteSelect(paneLocation, data.value);
                            }
                        });

                    }
                    return false;
                }
                else
                    return false; 
                
            }

            

            function editModule(paneLocation,paneId,urlTarget) {
                var index = $('#' + paneId + ' :selected').index();
                if (index != -1 && index > -1) {

                    
                    var id = $('#' + paneId + ' :selected').val();
                    var url = <%= getUrlToEdit()%>;
                    url += '&mID=';
                    url += id;
                    window.location = url;


                }
            }

            

            



            

	    </script>
    
</asp:Content>
