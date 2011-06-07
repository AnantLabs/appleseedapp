<%@ Register Assembly="Appleseed.Framework" Namespace="Appleseed.Framework.Web.UI.WebControls"
    TagPrefix="rbfwebui" %>
<%@ Control AutoEventWireup="false" Inherits="Appleseed.Content.Web.Modules.Portals"
    Language="c#" CodeBehind="Portals.ascx.cs" %>
<table align="center" border="0" cellpadding="2" cellspacing="0">
    <tr valign="top">
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr valign="top">
                    <td>
                        <asp:ListBox ID="portalList" runat="server" CssClass="NormalTextBox" DataSource="<%# portals %>"
                            DataTextField="Name" DataValueField="ID" Rows="8" Width="200"></asp:ListBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <rbfwebui:imagebutton id="EditBtn" runat="server" alternatetext="Edit selected portal"
                                        textkey="EDIT_PORTAL" onclick="EditBtn_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <rbfwebui:imagebutton id="DeleteBtn" runat="server" alternatetext="Delete selected portal"
                                        textkey="DELETE_PORTAL" onclick="DeleteBtn_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="SerializeBtn" runat="server" Text="Serialize selected portal"
                                        OnClick="SerializeBtn_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <p class="Normal">
                <rbfwebui:label id="ErrorMessage" runat="server" cssclass="Error" visible="false"></rbfwebui:label>
            </p>
        </td>
    </tr>
</table>
