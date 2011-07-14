<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HtmlEditModal.aspx.cs" Inherits="Appleseed.DesktopModules.CommunityModules.HTMLDocument.HtmlEditModal" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
</head>
<body style="background-image: none !important">
    <form id="htmleditmodalform" runat="server">
    
    <div>
        <%--<table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td align="left" class="Head">
                    <rbfwebui:Localize ID="Literal1" runat="server" Text="HTML Editor" TextKey="HTML_EDITOR">
                    </rbfwebui:Localize>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr noshade="noshade" size="1" />
                </td>
            </tr>
        </table>--%>
        <table border="0" cellpadding="4" cellspacing="0" width="98%">
            <tr>
                <td class="SubHead">
                    <%--<p>--%>
                    <rbfwebui:Localize ID="Literal2" runat="server" Text="Desktop HTML Content" TextKey="HTML_DESKTOP_CONTENT">
                    </rbfwebui:Localize><font face="ËÎÌå">:</font>
                    <br />
                    <div class="normal">
                        <asp:PlaceHolder ID="PlaceHolderHTMLEditor" runat="server"></asp:PlaceHolder>
                    </div>
                    <%--</p>--%>
                </td>
            </tr>
            
        </table>
        <p>
            <asp:PlaceHolder ID="PlaceHolderButtons" runat="server"></asp:PlaceHolder>
        </p>
    </div>
    
    </form>
</body>
</html>
