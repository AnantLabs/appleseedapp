<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table id="available_packages">
    <%foreach (var package in Model) { %>
    <tr>
        <td>
            <img src="<%: package.icon %>" alt="" />
        </td>
        <td>
            <%: package.name %>
        </td>
        <td>
            <%: package.version%>
        </td>
        <td>
            <%: package.author%>
        </td>
        <td>
            <a href="javascript:void(0);" onclick="installPackage('<%: package.name %>', '<%: package.source %>');">
                install</a>
        </td>
    </tr>
    <%} %>
</table>
<div id="installingDiv" style="display: none">
    <div class="ui-state-highlight ui-corner-all"><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span> This may take a few minutes, please wait until this dialog closes.</div>
    <br />
    <ul id="installingUl">
        <li>Starting installation...</li>
    </ul>
</div>
