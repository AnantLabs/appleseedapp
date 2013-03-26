<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="MvcContrib" %>
<script src="<%: Url.Resource("Scripts.SelfUpdater.js") %>" type="text/javascript"></script>
<script src="<%: Url.Resource("Scripts.jquery.signalR-1.0.1.min.js") %>" type="text/javascript"></script>
<link type="text/css" rel="stylesheet" href="<%: Url.Resource("Content.SelfUpdater.css") %>" />
<script src="/signalr/hubs" type="text/javascript"></script>
<div id="InstalationDiv">
    Getting the packages from the repository...
</div>

<script type="text/javascript">
    $(document).ready(function () {           

        $.ajax({
            url: '<%= Url.Action("InstallModule","Installation")%>',
            timeout: 90000,
            type: "POST",
            success: function (data) {
                $('#InstalationDiv').html(data);
            },
            error: function () {
                $('#InstalationDiv').html('Unable to get packages to Install. Please try again Later.');
            }
        });

    });
</script>