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
    <ul id="installingUl">
        <li>Installing packages...</li>
    </ul>
</div>
