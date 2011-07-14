<%@ Control Language="c#" Inherits="Appleseed.DesktopModules.CommunityModules.HTMLDocument.HtmlModule"
    CodeBehind="HtmlModule.ascx.cs" %>

<div id="HtmlModuleText" runat="server" >
    <asp:PlaceHolder ID="HtmlHolder" runat="server"></asp:PlaceHolder>
    <div id="HtmlModuleDialog" runat="server" style="display: none" title="Edit Html">
        <iframe id="HtmlMoudleIframe" runat="server" width="100%" height="100%" ></iframe>
    </div>   
</div>

