<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="MvcContrib" %>
<script src="<%: Url.Resource("Scripts.SelfUpdater.js") %>" type="text/javascript"></script>
<link type="text/css" rel="stylesheet" href="<%: Url.Resource("Content.SelfUpdater.css") %>" />

<div id="InstalationDiv">
    Getting the packages from the repository...
</div>

<script type="text/javascript">
$(document).ready(function () {
    
    $.ajax({
        url: "/SelfUpdater/Installation/InstallModule",
        type: "POST",
        success: function (data) {
            $('#InstalationDiv').html(data);            
        }
    });
    
});
</script>