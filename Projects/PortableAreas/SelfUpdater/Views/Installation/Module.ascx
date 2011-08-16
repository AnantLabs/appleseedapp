<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<table>
    <%foreach (var package in Model) { %>
    <tr>
        <td>
            <img src="<%= package.icon %>" alt="" />
        </td>
        <td>
            <%: package.name %>
        </td>
        <td>
            <%: package.version%>
        </td>
    </tr>
    <%} %>
</table>
