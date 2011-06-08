<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SelfUpdater.Models.InstallationState>>" %>
<%@ Import Namespace="MvcContrib" %>
<script type="text/javascript" language="javascript">
    window.jQuery || document.write('<script src="/aspnet_client/jQuery/jquery-1.5.1.min.js">\x3C/script>')
</script>
<script src="<%: Url.Resource("Scripts.UpdateModule.js") %>" type="text/javascript"></script>
<div class="installed_modules">
    <% if (Model.Count == 0) {%>
    There are no packages installed
    <%} else { %>
    <% foreach (var m in Model) {%>
    <div class="moduleinfo">
        <%= m.Installed.Id%>
        <%= m.Installed.Version%>
    </div>
    <div id="updateDiv">
        <%
           if (m.Update != null) {%>
        Update Available !
        <ul>
            <li>
                <%: m.Update.Id%>
                <%: m.Update.Version%>
                <a id="update<%: m.Update.Id %>" href="javascript:void(0)" onclick="updateModule('<%: m.Update.Id %>')">
                    Install update</a></li>
        </ul>
        <%} else {%>
        No Updates Available
        <%               }%>
        <hr />
        <%}           %>
    </div>
    <%} %>
</div>
<div id="upgradingDiv" style="display: none">
    <ul id="upgradingUl">
        <li>Upgrading module...<img src="" /></li>
    </ul>
</div>
