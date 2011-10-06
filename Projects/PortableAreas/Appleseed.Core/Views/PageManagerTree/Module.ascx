<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<form id="form1" runat="server">
    <div id="jsTree" class="demo">
    </div>
</form>

<script type="text/javascript">
    var nodeID;
    $(function () {
        $("#jsTree").jstree({
            "json_data": {
                "ajax": {
                    "type": "POST",
                    "url": "/Appleseed.Core/PageManagerTree/GetTreeData",
                    "data": function (n) {
                        return { ID: n.attr ? n.attr("ID") : 0 };
                    }
                }
            },
            "contextmenu": {
                "items": {
                    "ccp": false,
                    "rename": false,
                    "delete": false,
                    "create": false,
                    "newPage": {
                        "label": "Create",
                        "action":
                            function (obj) {
                                $.ajax({
                                    url: "/Appleseed.Core/PageManagerTree/create",
                                    type: 'POST',
                                    timeout: "100000",
                                    data: {
                                        "id": obj.attr("id").replace("pjson_", "")
                                    },
                                    success: function (data) {
                                        $("#jsTree").jstree("refresh", -1);
                                    }
                                });
                            }
                    },
                    "remove": {
                        "label": "Delete",
                        "action":
                            function (obj) {
                                $.ajax({
                                    url: "/Appleseed.Core/PageManagerTree/remove",
                                    type: 'POST',
                                    timeout: "100000",
                                    data: {
                                        "id": obj.attr("id").replace("pjson_", "")
                                    },
                                    success: function (data) {
                                        if (data.error == true) {
                                            alert(data.errorMess.toString());
                                        } else {
                                            $("#jsTree").jstree("refresh", -1);
                                        }
                                    }
                                });
                            }
                    },
                    "edit": {
                        "label": "Edit",
                        "action":
                            function (obj) {
                                $.ajax({
                                    url: "/Appleseed.Core/PageManagerTree/edit",
                                    type: 'POST',
                                    timeout: "100000",
                                    data: {
                                        "id": obj.attr("id").replace("pjson_", ""),
                                    },
                                    success: function (data) {
                                        window.location.href = data.url;

                                    }
                                });
                            }
                    }
                }
            },
            "crrm": {
                "move": {
                    "check_move": function (m) {
                        if (m.np.attr("id") == "jsTree") return false;
                        return true;
                    }
                }
            },
            "dnd": {
                "drop_target": false,
                "drag_target": false
            },
            "plugins": ["themes", "contextmenu", "json_data", "ui", "crrm", "dnd"]
        })
        .bind("move_node.jstree", function (e, data) {
            var result;
            if ((typeof (data.rslt.or.attr("id"))) == 'undefined') {
                result = -1;
            } else {
                result = data.rslt.or.attr("id").replace("pjson_", "");
            }
            $.ajax({
                url: "/Appleseed.Core/PageManagerTree/moveNode",
                type: 'POST',
                timeout: "100000",
                data: {
                    "pageId": data.rslt.o.attr("id").replace("pjson_", ""),
                    "newParent": data.rslt.np.attr("id").replace("pjson_", ""),
                    "idOldNode": result
                },
                success: function (data) {
                }
            });
        });
    });
</script>

