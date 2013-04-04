<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SelfUpdater.Models.NugetPackagesModel>" %>
<%@ Import Namespace="MvcContrib" %>
<script src="<%: Url.Resource("Scripts.SelfUpdater.js") %>" type="text/javascript"></script>
<script src="<%: Url.Resource("Scripts.jquery.signalR-1.0.1.min.js") %>" type="text/javascript"></script>
<link type="text/css" rel="stylesheet" href="<%: Url.Resource("Content.SelfUpdater.css") %>" />
<script src="/signalr/hubs" type="text/javascript"></script>
<div id="InstalationDiv" style="margin-bottom: 25px;">
    <% Html.RenderPartial("InstallModule", Model.Install); %>
</div>

<div style="display:inline">
    <span class="ModuleTitle">
        <span class="editTitle">Installed Packages </span>
    </span>
    <hr>
</div>

<div id="UpdateDiv">
    <% Html.RenderPartial("UpdateModule", Model.Updates); %>
    <%--<% Html.RenderAction("UpdateModule","Updates"); %>--%>
</div>

<input type="button" value="Apply Changes" onclick="InstallPackages()" />


<script type="text/javascript">

    var instalationPackages = '<%= Url.Action("InstallPackages")%>';
    
</script>