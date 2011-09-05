<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SelfUpdater.Models.InstallationState>>" %>
<%@ Import Namespace="MvcContrib" %>
<script src="<%: Url.Resource("Scripts.SelfUpdater.js") %>" type="text/javascript"></script>
<link type="text/css" rel="stylesheet" href="<%: Url.Resource("Content.SelfUpdater.css") %>" />
<div>
    <% if (Model.Count == 0) {%>
    There are no packages installed
    <%} else { %>
    <table id="installed_packages">
        <tr>
            <th>
            </th>
            <th>
                Current
            </th>
            <th>
                Update
            </th>
            <th>
            </th>
        </tr>
        <% foreach (var m in Model) {%>
        <tr class="installed_package">
            <td class="package_icon">
                <img src="<%= m.Installed.IconUrl %>" alt="" />
            </td>
            <td class="package_info">
                <%= m.Installed.Id%>&nbsp;<%= m.Installed.Version%>
            </td>
            <td class="package_update_info">
                <%
               if (m.Update != null) {%>
                <%: m.Update.Id%>&nbsp;<%: m.Update.Version%>&nbsp;<a id="schedule<%: m.Update.Id %>"
                    href="javascript:void(0)" onclick="updatePackage('<%: m.Update.Id %>', true,'<%: m.Update.ProjectUrl %>','<%: m.Update.Version.ToString() %>' )"> Schedule
                    update</a> <a id="unschedule<%: m.Update.Id %>" href="javascript:void(0)" onclick="updatePackage('<%: m.Update.Id %>', false, '<%: m.Update.ProjectUrl %>','<%: m.Update.Version.ToString() %>')"
                        style="display: none">Unschedule update</a>
                <%} else { %>
                (none)
                <%} %>
                <%if (m.Scheduled) { %>
                <script type="text/javascript">
                    updatePackage('<%: m.Installed.Id %>', true, '<%: m.Update.ProjectUrl %>', '<%: m.Update.Version.ToString() %>');
                </script>
                <%}%>
                <%}%>
            </td>
        </tr>
        <%} %>
    </table>
</div>
<% if (Model.Count > 0) {%>
<div class="installed_packages_action">
    <input type="button" id="apply" onclick="javascript:applyUpdates();" value="Apply updates!"
        class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" />
</div>
<%} %>
<div id="upgradingDiv" style="display: none">
    <ul id="upgradingUl">
    </ul>
</div>
