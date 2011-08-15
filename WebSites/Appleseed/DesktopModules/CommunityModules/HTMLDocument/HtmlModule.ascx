<%@ Control Language="c#" Inherits="Appleseed.DesktopModules.CommunityModules.HTMLDocument.HtmlModule"
    CodeBehind="HtmlModule.ascx.cs" %>

<div style="position: relative; overflow: auto;">
    <div id="HtmlModuleText" runat="server"></div>
        <asp:PlaceHolder ID="HtmlHolder" runat="server"></asp:PlaceHolder>
        <div id="HtmlModuleDialog" runat="server" style="display: none" title="Edit Html">
            <iframe id="HtmlMoudleIframe" runat="server" ></iframe>
        </div>   
    
</div>
