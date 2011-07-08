<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignInSocialNetwork.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SignIn.SignInSocialNetwork" %>


<input type="hidden" runat="server" id="appidfacebook"/>
<div id="fb-root"></div>
 <script type="text/javascript" src="http://connect.facebook.net/en_US/all.js"></script>
 
<table cellpadding="0" cellspacing="0" class="signInContainer">
    <tbody>
        <tr>
            <td>
                <div runat="server" id="loginfb_div">
                    <fb:login-button autologoutlink="true" perms="email">Login with Facebook</fb:login-button>
                    
                </div>
                <%--<asp:Label runat="server" ForeColor="Red" ID="errfb" Visible="false">Facebook settings are not correct</asp:Label>--%>
            </td>
        </tr>
         <tr>
            <td>
                <div runat="server" id="logintwit_div">
                    <% if (!string.IsNullOrEmpty(Request.QueryString.Get("iframe"))) {%>
                        <a id="twit" class="twitterlink" href="#" onclick="gotoTwitter('<%= getTwitterLink() %>');return false;"><img src="../../../images/sign-in-with-twitter-l.png" </a>
                    <%} else { %>
                        <asp:HyperLink runat="server" ImageUrl="~/images/sign-in-with-twitter-l.png" ID="LogIn" class="twitterlink" ></asp:HyperLink>
                    <%} %>
                </div>
                <%--<asp:Label runat="server" ForeColor="Red" ID="errtwit" Visible="false">Twitter settings are not correct</asp:Label>--%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" Visible="false" EnableViewState="false" ForeColor="Red"
                    Text="Ha ocurrido un error. Intente nuevamente más tarde." />
                <asp:Label ID="lblPassSent" runat="server" Visible="false" EnableViewState="false" ForeColor="Red"
                    Text="La contrase&ntilde;a ha sido enviada a la direcci&oacute;n ingresada. Revise su casilla de correos para poder acceder al sitio web." />
                <asp:Label ID="lblPassSentErr" runat="server" Visible="false" EnableViewState="false" ForeColor="Red"
                    Text="La contrase&ntilde;a ha sido enviada a la direcci&oacute;n ingresada. Revise su casilla de correos para poder acceder al sitio web." />
                
            </td>
        </tr>
    </tbody>
</table>
<asp:HiddenField ID="appId" runat="server" />
<script type="text/javascript">
    var appId = $(".signInContainer").next().val();
    FB.init({
        appId: appId,
        status: false, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });

    FB.Event.subscribe('auth.login', function (response) {
        window.location.reload();
    });

    function gotoTwitter(link) {
        window.parent.location = link 
    }
    
 </script>



