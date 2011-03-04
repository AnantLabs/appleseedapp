<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<InstallationState>>" %>

<%@ Import Namespace="Appleseed.Core.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Checking For Updates</title>
</head>
<body>
    <div>
        <h1>
            Currently Installed</h1>
        <% foreach (var m in Model) {%>
        <div>
            <%= m.Installed.Id %>
            <%= m.Installed.Version %>
        </div>
        <%
               if (m.Update != null)
               {%>
        <h2>
            Update Available</h2>
        <ul>
            <li>
                <%=m.Update.Id %>
                <%= m.Update.Version %>
                <% Html.ActionLink("Install", "Upgrade")%></li>
        </ul>
        <%               }
               else
               {%>
        <h2>
            No Updates Available</h2>
        <%               }
}
        %>
    </div>
</body>
</html>
