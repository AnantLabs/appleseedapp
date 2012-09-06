<%@ Page Language="C#" Inherits="Appleseed.Framework.Web.UI.Page" MasterPageFile="~/Shared/SiteMasterDefault.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="div_ev_Table">
        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td align="left" class="Head">
                    <label class="Normal"><%= Appleseed.Framework.General.GetString("CHANGE_PWD_HEADER", "Change your password")%></label>                    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr noshade="noshade" size="1" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td class="Normal">
                   <label id="lblMessage" class="Normal"></label>
                </td>
            </tr>
            <tr  id="trFields">
                <td>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td><label class="Normal"><%= Appleseed.Framework.General.GetString("CHANGE_PWD_ENTER_NEW_PWD", "New password")%></label></td>
                            <td><input type="password" id="txtPass" /></td>
                        </tr>
                        <tr>
                            <td><label class="Normal"><%= Appleseed.Framework.General.GetString("CHANGE_PWD_ENTER_NEW_PWD_AGAIN", "New password again")%></label></td>     
                            <td><input type="password" id="txtPass2" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <p>
            <input type="button" id="SaveBtn" class="CommandButton" value="<%= Appleseed.Framework.General.GetString("SAVE", "Save")%>"/>
            
            <input type="button" id="CancelBtn" class="CommandButton" value="<%= Appleseed.Framework.General.GetString("CANCEL", "Cancel")%>"/>

            <input type="button" id="GoHomeBtn" class="CommandButton" value="<%= Appleseed.Framework.General.GetString("CHANGE_PWD_GO_TO_HOME_PAGE", "Go to home page")%>"/>            

        </p>
    </div>
</asp:Content>
