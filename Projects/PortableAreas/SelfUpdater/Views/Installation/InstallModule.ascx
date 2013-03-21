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
            <a href="javascript:void(0);" onclick="installPackage('<%: package.name %>', '<%: package.source %>', '<%: package.version %>' );">
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

<script type="text/javascript">
function installPackage(packageId, source, version) {

    $('#installingDiv').dialog({
        modal: true,
        closeOnEscape: false,
        closeText: '',
        resizable: false,
        title: 'Install package',
        width: 550,
        height: 200,
        open: function (event, ui) {
            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
        }

    });
    var status = false;
    $.ajax({
        url: '<%= Url.Action("InstallPackage","Installation")%>',
        type: "POST",
        data: { packageId: packageId, source: source, version: version },
        timeout: 3600000,
        success: function (data) {
            //        var xhr;
            //        var reloading = false;
            //        var fn = function () {
            //            if (!reloading) {
            //                if (xhr && xhr.readystate != 4) {
            //                    xhr.abort();
            //                }
            //                xhr = $.post('/SelfUpdater/Updates/Status').success(function (data) {
            //                    if (data.online) {
            //                        $('<li>Reloading site...</li>').appendTo('#installingUl');
            //                        reloading = true;
            //                        window.location.reload();
            //                    }
            //                });
            //            }
            //        };

            //        var interval = setInterval(fn, 10000);     

            if (data.NugetLog != null && data.NugetLog != '') {
                $('#installingUl').html(data.NugetLog);
            }

            $.ajax({
                url: '<%= Url.Action("RestartSite","Updates")%>',
                type: "POST",
                success: function (data) {
                    clearInterval(myinterval);
                },
                error: function () {

                }
            });

            var timeOutStatus = false;
            var cant = 0;
            var interval = setInterval(function () {

                $.ajax({
                    url: '<%= Url.Action("Status","Updates")%>',
                    type: "POST",
                    timeout: 1000,
                    success: function (data) {

                        if (data) {


                            window.location = window.location.href;
                            clearInterval(interval);
                        }
                    },
                    error: function () {
                        if (!timeOutStatus) {
                            timeOutStatus = true;
                            $('#installingUl').append('<li>Reloading site<span id="TimeOutPointsInstall">...</span></li>')
                        }
                        cant = cant + 1;
                        if (cant == 4) {
                            cant = 1;
                        }
                        var puntos = "";
                        if (cant == 1) {
                            puntos = '.';
                        }
                        if (cant == 2) {
                            puntos = '..';
                        }
                        if (cant == 3) {
                            puntos = '...';
                        }
                        $('#TimeOutPointsInstall').html(puntos);
                    }
                });
            }, 2000);
        },
        error: function (data, error) {
            console.log(data.responseText);
            $('#installingDiv').dialog("close");
            alert("There has been an error installing the package. Please check the log.");
            clearInterval(myinterval);
        }
    });

    var myinterval = setInterval(function () {

        $.ajax({
            url: '<%= Url.Action("NugetStatus","Updates")%>',
            type: "POST",
            async: false,
            success: function (data) {
                if (data != null && data != '') {
                    $('#installingUl').html(data);
                }
            }
        });
        if (status) {
            clearInterval(myinterval);
        }
    }, 2000);


    return false;

}

</script>