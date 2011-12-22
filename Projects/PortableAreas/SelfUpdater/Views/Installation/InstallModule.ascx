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
    <div style="background-color: Yellow">Warning: This may take a few minutes, please wait until this dialog closes.</div>
    <br />
    <ul id="installingUl">
        <li>Starting installation...</li>
    </ul>
</div>
