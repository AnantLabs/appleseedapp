<%@ Page Language="C#" Inherits="Appleseed.Framework.Web.UI.Page" MasterPageFile="~/Shared/SiteMasterDefault.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <br />
    <label id="Message" class="Error"></label>
    <br />
    <div id="Fields">
        <label id="EmailLabel"><%= Appleseed.Framework.General.GetString("EmailAddress", "Email Address") %>: </label><input type="text" id="UsersEmail" class="NormalTextBox" />
        <br />
        <input type="button" id="SendPasswordBtn" class="CommandButton" value="<%= Appleseed.Framework.General.GetString("SendEmail", "Send Email") %>" onclick="sendPasswordToken()" />
    </div>
     


     <script type="text/javascript">

         function sendPasswordToken() {

             var email = $('#UsersEmail').val();

             if (email != '') {

                 $.ajax({
                     url: '<%= Url.Action("sendPasswordToken")%>',
                     type: 'POST',
                     data: {
                         "email": email
                     },
                     success: function (data) {
                         $('#Message').text(data.Message);
                         $('#Fields').hide();
                     },
                     error: function (data) {
                         $('#Message').text(data.responseText);

                     }
                 });
             }
             else {
                 $('#Message').text('You must enter an email');
             }
         }
     
     </script>
    

</asp:Content>