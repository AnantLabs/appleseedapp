/// <reference path="/Scripts/jquery-1.5-vsdoc.js" />




function updateModule(moduleId, schedule) {
   
   var actionurl = '/SelfUpdater/Updates/DelayedUpgrade';
   if (schedule == false) {
       actionurl = '/SelfUpdater/Updates/RemoveDelayedUpgrade';
       $('#schedule' + moduleId).show();
       $('#unschedule' + moduleId).hide();
   } else {
       $('#schedule' + moduleId).hide();
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

    $.ajax({
        url: '/SelfUpdater/Updates/ApplyUpdates',
        data: {},
        dataType: 'json',
        timeout: 1200000,
        success: function (data) {
            $('<li>' + data.msg + '</li>').appendTo('#upgradingUl');
            window.location.reload();
        },
        error: function (data) {
            trace(data);
            $('#upgradingDiv').dialog("close");
            alert("Communication error");
        }
    });
}