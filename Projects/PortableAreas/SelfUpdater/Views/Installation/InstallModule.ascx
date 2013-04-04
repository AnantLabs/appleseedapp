<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table id="available_packages">
    <%foreach (var package in Model)
      { %>
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
        <%--<td>
            <a href="javascript:void(0);" onclick="installPackage('<%: package.name %>', '<%: package.source %>', '<%: package.version %>' );">
                install</a>
        </td>--%>
        <td>
            <input class="InstallChecker" type="checkbox" />
            <input type="hidden" class="PackageName" value="<%: package.name %>"/>
            <input type="hidden" class="PackageSource" value="<%: package.source %>"/>
            <input type="hidden" class="PackageVersion" value="<%: package.version %>"/>
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
    <div id="TestingSignalR"></div>
</div>

<script type="text/javascript">
    
    
    
    function getCurrentPage() {
        $.ajax({
            type: "GET",
            url: window.location.href,
            success: function () {}
        });
    }


    $(function () {
        
        var updaterHub = $.connection.selfUpdaterHub;

        updaterHub.client.nuevoProcentaje = function (message) {
            $('#TestingSignalR').append('<span>' + message + '</span><br/>');
            $("#installingDiv").scrollTop($("#installingDiv")[0].scrollHeight);
        };

        updaterHub.client.reloadPage = function(data) {
            getCurrentPage();
            setTimeout(window.location = window.location.href, 5000);
        };

        updaterHub.client.openPopUp = function (data) {
            $('#installingDiv').dialog("open");
            console.log('SingalR opening pop up');
        };
        
        updaterHub.client.console = function (data) {
            console.log(data);
        };

        $.connection.hub.start().done(function () {
            console.log('SignalR loaded');
        });
        

        $('#installingDiv').dialog({
            modal: true,
            closeOnEscape: false,
            closeText: '',
            resizable: false,
            title: 'Install package',
            width: 550,
            height: 600,
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
            },
            autoOpen: false

        });

    });


    

</script>