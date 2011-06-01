<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignInFacebook.ascx.cs" Inherits="Appleseed.DesktopModules.CoreModules.SignIn.SignInFacebook" %>
<div id="fb-root"></div>
 <script type="text/javascript" src="http://connect.facebook.net/en_US/all.js"></script>
 
<table cellpadding="0" cellspacing="0" class="signInContainer">
    <tbody>
        <tr>
            <td>
                <div runat="server" id="loginfb_div">
                    <fb:login-button autologoutlink="true" perms="email">Login with Facebook</fb:login-button>
                </div>
                <asp:Label runat="server" ForeColor="Red" ID="errfb" Visible="false">Facebook settings are not correct</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" cellpadding="6" cellspacing="0" style="width: 100%">
                    <tbody>
                        <tr>
                            <td rowspan="3" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" class="signInBody">
                                    <tbody>
                                        <tr>
                                            <td height="17" valign="top" width="60%">
                                                <span class="Normal">E-Mail:</span>
                                                <asp:RequiredFieldValidator  Font-Size="10px" ID="valReqEmail" runat="server" ControlToValidate="txtEmail"
                                                    Display="dynamic" ErrorMessage="Debes ingresar un e-mail." Text="*" ValidationGroup="SignIn" />
                                                <asp:RequiredFieldValidator  Font-Size="10px" ID="rfvSendPwd" runat="server" ControlToValidate="txtEmail"
                                                    Display="dynamic" ErrorMessage="Debes ingresar un e-mail." Text="*" ValidationGroup="SendPwd" />
                                                <asp:CustomValidator ID="valExistsEmail" runat="server" Display="Dynamic" ErrorMessage="Ud. no est&aacute; registrado en nuestro sitio web. Haga click en 'Registrarse' para completar el formulario y realizar el registro en el sitio."
                                                    Text="*" Font-Size="10px" ValidationGroup="SignIn" OnServerValidate="valExistsEmail_ServerValidate" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" Width="154px"  />
                                                <br />
                                                <asp:LinkButton ID="lbtnSendPassword" runat="server" Font-Size="7pt" OnClick="lbtnSendPassword_Click"
                                                    Text="(Enviar contrase&ntilde;a)" CausesValidation="true" ValidationGroup="SendPwd" />
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td height="20">
                                                <span class="Normal">Password:</span>
                                                <asp:RequiredFieldValidator ID="valReqPassword" runat="server" ControlToValidate="txtPassword"
                                                    Display="dynamic" ErrorMessage="Debes ingresar una contrase&ntilde;a" Text="*"
                                                    ValidationGroup="SignIn"  Font-Size="10px" />
                                                <asp:CustomValidator  Font-Size="10px" ID="valPassword" runat="server" Display="Dynamic" ErrorMessage="La contrase&ntilde;a ingresada es incorrecta. Si no la recuerdas, haz click en 'Enviar contrase&ntilde;a' y te la enviaremos a tu casilla de correo."
                                                    Text="*" ValidationGroup="SignIn" OnServerValidate="valPassword_ServerValidate" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPassword" runat="server" Width="154px" TextMode="Password" />&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="imgLogin" runat="server" Text="Sign-In" OnClick="imgLogin_Click" CssClass="imgLoginStyle"  ValidationGroup="SignIn" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="18">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td height="25" width="41%">
                                                                <span class="Normal">Remember Login</span>
                                                            </td>
                                                            <td width="59%">
                                                                <span class="Normal">
                                                                    <asp:CheckBox ID="chkSaveLogin" runat="server" Checked="true" />
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <asp:Button ID="lnkRegister" Text="Register" runat="server" OnClick="lnkRegister_Click" CssClass="lnkRegisterStyle"/>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </tbody>
                </table>
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
                <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="SignIn" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="SendPwd" />
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

 </script>



