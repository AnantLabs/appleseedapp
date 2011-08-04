/// <reference path="/Scripts/jquery-1.5-vsdoc.js" />




function updateModule(moduleId) {

    $('#upgradingDiv').dialog({
        modal: true,
        closeOnEscape: false,
        closeText: '',
        resizable: false,
        title: 'Upgrading site',
        open: function (event, ui) {
            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
        }

    });

    $.ajax({
        url: '/SelfUpdater/Updates/DelayedUpgrade',
        data: {
            packageId: moduleId
        },
        dataType: 'json',
        timeout: 1200000,
        success: function (data) {
            $('<li>' + data.msg + '</li>').appendTo('#upgradingUl');
            if (data.updated) {
                $('<li>Reloading site...</li>' + data.msg).appendTo('#upgradingUl');
                window.location.reload();
            } else {
                $('#upgradingDiv').closest('.ui-dialog').find('.ui-dialog-titlebar-close').show();
            }
        },
        error: function (data) {
            trace(data);
            $('#upgradingDiv').dialog("close");
            alert("Communication error");
        }
    });

    return false;
}