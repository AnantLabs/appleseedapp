/// <reference path="/Scripts/jquery-1.5-vsdoc.js" />




function updateModule(moduleId, schedule) {

    var actionurl = '/SelfUpdater/Updates/DelayedUpgrade';
    if (schedule == false) {
        actionurl = '/SelfUpdater/Updates/RemoveDelayedUpgrade';

        $('#schedule' + moduleId).show();
        $('#schedule' + moduleId).parent('li').removeClass('ui-state-highlight ui-corner-all');

        $('#unschedule' + moduleId).hide();

    } else {
        $('#schedule' + moduleId).hide();
        $('#schedule' + moduleId).parent('li').addClass('ui-state-highlight ui-corner-all');

        $('#unschedule' + moduleId).show();
    }

    $.ajax({
        url: actionurl,
        data: {
            packageId: moduleId
        },
        dataType: 'json',
        timeout: 1200000,
        success: function (data) {

        },
        error: function (data) {
            trace(data);
            $('#upgradingDiv').dialog("close");
            alert("Communication error");
        }
    });

    return false;
}

function applyUpdates() {

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

    $.post('/SelfUpdater/Updates/ApplyUpdates')
    .success(function () {
        $('<li>Applying updates...</li>').appendTo('#upgradingUl');

        var xhr;
        var reloading = false;
        var fn = function () {
            if (!reloading) {
                if (xhr && xhr.readystate != 4) {
                    xhr.abort();
                }
                xhr = $.post('/SelfUpdater/Updates/Status').success(function (data) {
                    if (data.online) {
                        $('<li>Reloading site...</li>').appendTo('#upgradingUl');
                        reloading = true;
                        window.location.reload();
                    }
                });
            }
        };

        var interval = setInterval(fn, 10000);
    })
    .error(function () {
        trace(data);
        $('#upgradingDiv').dialog("close");
        alert("Communication error");
    });


    return false;
}

