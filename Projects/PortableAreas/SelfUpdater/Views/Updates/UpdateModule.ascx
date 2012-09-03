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
<%--            <td class="package_update_info">
                <%
               if (m.Update != null) {%>
                <%: m.Update.Id%>&nbsp;<%: m.Update.Version%>&nbsp;<a id="schedule<%: m.Update.Id %>"
                    href="javascript:void(0)" onclick="scheduleUpdatePackage('<%: m.Update.Id %>', true,'<%: m.Source %>','<%: m.Update.Version.ToString() %>' )"> Schedule
                    update</a> <a id="unschedule<%: m.Update.Id %>" href="javascript:void(0)" onclick="scheduleUpdatePackage('<%: m.Update.Id %>', false, '<%: m.Source %>','<%: m.Update.Version.ToString() %>')"
                        style="display: none">Unschedule update</a>
                <%} else { %>
                (none)
                <%} %>
                <%if (m.Scheduled) { %>
                <script type="text/javascript">
                    updatePackage('<%: m.Installed.Id %>', true, '<%: m.Source %>', '<%: m.Update.Version.ToString() %>');
                </script>
                <%}%>
                <%}%>
            </td>--%>
            <td class="package_update_info">
            <%
               if (m.Update != null) {%>
                <%: m.Update.Id%>&nbsp;<%: m.Update.Version%>&nbsp;
                <a id="update<%: m.Update.Id %>" href="javascript:void(0);" onclick="updatePackage('<%: m.Update.Id %>', '<%: m.Source %>');">
                    update</a>
                    <%} else { %>
                (none)
                <%} %>
            </td>
        </tr>
        <%} %>
    </table>
        <%} %>
</div>
<%--<% if (Model.Count > 0) {%>
<div class="installed_packages_action">
    <input type="button" id="apply" onclick="javascript:applyUpdates();" value="Apply updates!"
        class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" />
</div>
<% } %>
--%><div id="upgradingDiv" style="display: none">
    <div class="ui-state-highlight ui-corner-all"><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span> This may take a few minutes, please wait until this dialog closes.</div>
    <br />
    <ul id="upgradingUl">
        <li>Starting update...</li>
    </ul>
</div>

<script type="text/javascript">

    function updatePackage(packageId, source) {

        $('#upgradingDiv').dialog({
            modal: true,
            closeOnEscape: false,
            closeText: '',
            resizable: false,
            title: 'Update package',
            width: 550,
            height: 200,
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
            }

        });


        $.ajax({
            url: '<%= Url.Action("Upgrade","Updates")%>',
            type: "POST",
            data: { packageId: packageId, source: source },
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

                if (data.updated) {

                    if (data.NugetLog != null && data.NugetLog != '') {
                        $('#upgradingUl').html(data.NugetLog);
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
                                    $('#upgradingUl').append('<li>Reloading site<span id="TimeOutPointsUpgrade">...</span></li>');
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
                                $('#TimeOutPointsUpgrade').html(puntos);

                            }
                        });
                    }, 2000);
                }
                else {
                    clearInterval(myinterval);
                    $('#upgradingUl').append('<li>There has been an error upgrading the package.</li>');
                    $('#upgradingUl').append('<li>Please try again later.</li>')
                }
            },
            error: function () {
                trace(data);
                $('#upgradingDiv').dialog("close");
                alert("Communication error");
            }
        });


        var myinterval = setInterval(function () {

            $.ajax({
                url: '<%= Url.Action("NugetStatus","Updates")%>',
                type: "POST",
                async: false,
                success: function (data) {
                    if (data != null && data != '') {
                        $('#upgradingUl').html(data);
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