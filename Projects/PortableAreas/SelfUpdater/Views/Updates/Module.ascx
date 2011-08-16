<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SelfUpdater.Models.InstallationState>>" %>
<%@ Import Namespace="MvcContrib" %>
<script src="<%: Url.Resource("Scripts.UpdateModule.js") %>" type="text/javascript"></script>
<link type="text/css" rel="stylesheet" href="<%: Url.Resource("Content.SelfUpdater.css") %>" />

<div class="installed_modules">
    <% if (Model.Count == 0) {%>
    There are no packages installed
    <%} else { %>
    <table>
        <tr>
            <th></th>
            <th>Current</th>
            <th>Update</th>
            <th></th>
        </tr>
        <% foreach (var m in Model) {%>
        <tr class="installed_module">
            <td class="module_icon">
                <%= m.Installed.IconUrl %>
            </td>
            <td class="module_info">
                <%= m.Installed.Id%>&nbsp;<%= m.Installed.Version%>
            </td>
            <td class="module_update_info">
                <%
               if (m.Update != null) {%>
                <%: m.Update.Id%>&nbsp;<%: m.Update.Version%>&nbsp;<a id="schedule<%: m.Update.Id %>"
                    href="javascript:void(0)" onclick="updateModule('<%: m.Update.Id %>', true)"> Schedule
                    update</a> <a id="unschedule<%: m.Update.Id %>" href="javascript:void(0)" onclick="updateModule('<%: m.Update.Id %>', false)"
                        style="display: none">Unschedule update</a>
                <%} else { %>
                (none)
                <%} %>
                <%if (m.Scheduled) { %>
                <script type="text/javascript">
                    updateModule('<%: m.Installed.Id %>', true);
                </script>
                <%}%>
                <%}%>
            </td>
        </tr>
        <%} %>
    </table>
</div>
<div class="installed_modules_action">
    <input type="button" id="apply" onclick="javascript:applyUpdates();" value="Apply updates!"
        class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" />
</div>
<div id="upgradingDiv" style="display: none">
    <ul id="upgradingUl">
    </ul>
</div>
