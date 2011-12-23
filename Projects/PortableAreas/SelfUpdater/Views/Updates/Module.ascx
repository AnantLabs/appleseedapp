<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SelfUpdater.Models.InstallationState>>" %>
<div id="UpdateDiv">
    Getting the installed packages...
</div>

<script type="text/javascript">
$(document).ready(function () { 
    

    $.ajax({
        url: "/SelfUpdater/Updates/UpdateModule",
        type: "POST",
        timeout:90000,
        success: function (data) {
            $('#UpdateDiv').html(data);
        },
        error: function () {
            $('#UpdateDiv').html('Unable to get The installed packages. Please try again Later.');
        }
    });
});
</script>