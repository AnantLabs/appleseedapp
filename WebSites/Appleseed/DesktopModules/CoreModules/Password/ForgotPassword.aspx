<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Appleseed.DesktopModules.CoreModules.Password.ForgotPassword"
 MasterPageFile="~/Shared/SiteMasterDefault.master" %>

    

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    
    <table>
        <tr>
            <td>
                <rbfwebui:Label ID="Message" runat="server" CssClass="Error"></rbfwebui:Label>
            </td>    
        </tr>    
        <tr >
            <td>
                <label id="EmailLabel" runat="server"></label>
            </td>
            <td>
                <asp:TextBox ID="email" runat="server" Columns="24" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
     </table>
     <br />
     <rbfwebui:Button ID="SendPasswordBtn" runat="server" CssClass="CommandButton" EnableViewState="False" />
     
    

</asp:Content>

