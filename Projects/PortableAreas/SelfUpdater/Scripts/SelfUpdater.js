/// <reference path="/Scripts/jquery-1.5-vsdoc.js" />




function scheduleUpdatePackage(packageId, schedule, source, version) {

    var actionurl = '/SelfUpdater/Updates/DelayedUpgrade';
    if (schedule == false) {
        actionurl = '/SelfUpdater/Updates/RemoveDelayedUpgrade';

        $('#schedule' + packageId).show();
        $('#schedule' + packageId).parents('tr').first().removeClass('ui-state-highlight ui-corner-all');

        $('#unschedule' + packageId).hide();

    } else {
        $('#schedule' + packageId).hide();
        $('#schedule' + packageId).parents('tr').first().addClass('ui-state-highlight ui-corner-all');

        $('#unschedule' + packageId).show();
    }

    $.ajax({
        url: actionurl,
        data: {
            packageId: packageId,
            source: source,
            version: version
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

function installPackage(packageId, source) {

    $('#installingDiv').dialog({
        modal: true,
        closeOnEscape: false,
        closeText: '',
        resizable: false,
        title: 'Install package',
        open: function (event, ui) {
            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
        }

    });
    var status = false;
    $.post('/SelfUpdater/Installation/InstallPackage', { packageId: packageId, source: source })
    .success(function () {

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
        $('#installingUl').append('<li>The instalation has finished.</li>');
        $('#installingUl').append('<li>Waiting to reload site..</li>');

        var interval = setInterval(function () {

            $.ajax({
                url: "/SelfUpdater/Updates/Status",
                type: "POST",
                success: function (data) {

                    if (data) {

                        $('#installingUl').append('<li>Reloading site...</li>')
                        window.location = window.location.href;
                        clearInterval(interval);
                    }
                },
                error: function () {
                    alert('error');
                }
            });
        }, 5000);

    })
    .error(function () {
        trace(data);
        $('#installingDiv').dialog("close");
        alert("Communication error");
    });

    var myinterval = setInterval(function () {

        $.ajax({
            url: "/SelfUpdater/Updates/NugetStatus",
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

function updatePackage(packageId, source) {

    $('#upgradingDiv').dialog({
        modal: true,
        closeOnEscape: false,
        closeText: '',
        resizable: false,
        title: 'Update package',
        open: function (event, ui) {
            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
        }

    });

    $.post('/SelfUpdater/Updates/Upgrade', { packageId: packageId, source: source })
    .success(function () {

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

        $('#upgradingUl').append('<li>The instalation has finished.</li>');
        $('#upgradingUl').append('<li>Waiting to reload site..</li>');
        var interval = setInterval(function () {

            $.ajax({
                url: "/SelfUpdater/Updates/Status",
                type: "POST",
                success: function (data) {

                    if (data) {

                        $('#upgradingUl').append('<li>Reloading site...</li>')
                        window.location = window.location.href;
                        clearInterval(interval);
                    }
                },
                error: function () {
                    alert('error');
                }
            });
        }, 5000);

    })
    .error(function () {
        trace(data);
        $('#upgradingDiv').dialog("close");
        alert("Communication error");
    });


    var myinterval = setInterval(function () {

        $.ajax({
            url: "/SelfUpdater/Updates/NugetStatus",
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
