<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="PageManagerTree" %>
<script type="text/javascript" src="<%= Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/PageManagerTree/Scripts/jquery.jstree.js") %>"></script>
<link type="text/css" rel="stylesheet" href="<%: Appleseed.Framework.HttpUrlBuilder.BuildUrl("~/aspnet_client/jQuery/jsTree/themes/default/style.css") %>" /> 


<form id="form1" runat="server">
    <div id="jsTree" class="demo">
        

    </div>
</form>
<div>
    <input type="button" value="<%: Appleseed.Framework.General.GetString("ADDTAB") %>" onclick="AddNewPage();"/>
    <input type="button" value="<%: Appleseed.Framework.General.GetString("RESET_SEE_CHANGES") %>" onclick="seeChanges();"/>
</div>


<script type="text/javascript">
    var nodeID;
    $(function () {
        $("#jsTree")
//        .bind("loaded.jstree", function (e, data) {
//            data.inst.open_all('#pjson_0'); // -1 opens all nodes in the container
//        })
        .jstree({
            "json_data": {
                "ajax": {
                    "type": "POST",
                    "url": '<%= Url.Action("GetTreeData","PageManagerTree")%>',
                    "data": function (n) {
                        return { ID: n.attr ? n.attr("ID") : 0 };
                    }
                }
            },
            "contextmenu": {
                "items": {
                    "edit": {
                        "label": '<%: Appleseed.Framework.General.GetString("EDIT") %>',
                            "action":
                                function (obj) {
                                    $.ajax({
                                        url: '<%= Url.Action("edit","PageManagerTree")%>',
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
                        
                    },
                    "ccp": false,
                    "remove": false,
                    "rename": {
                        "label": '<%: Appleseed.Framework.General.GetString("RENAME") %>',
                        "action": function (obj) { this.rename(obj);
                                                    
                         },
                    },
                    "delete": {
                        "label": '<%: Appleseed.Framework.General.GetString("DELETE") %>',
                        "action":
                            function (obj) {
                                
                                var agree=confirm('<%: Appleseed.Framework.General.GetString("CONFIRM_DELETE") %>');
                                if (agree)
                                    deletePage(obj.attr("id").replace("pjson_", ""));
                                else
                                    return false ; 

                                
                                
                            }
                    },
                    "create": {
                            "label": '<%: Appleseed.Framework.General.GetString("CREATE") %>',
                            "action":
                                function (obj) {
                                    $.ajax({
                                        url: '<%= Url.Action("create","PageManagerTree")%>',
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
            "core": { "initially_open" : ["pjson_0"]} ,

            "plugins": ["themes", "contextmenu", "json_data", "ui", "crrm", "dnd","core"]
        })
        .bind("move_node.jstree", function (e, data) {
            var result;
            if ((typeof (data.rslt.or.attr("id"))) == 'undefined') {
                result = -1;
            } else {
                result = data.rslt.or.attr("id").replace("pjson_", "");
            }
            $.ajax({
                url: '<%= Url.Action("moveNode","PageManagerTree")%>',
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
        })
        .bind("rename.jstree",function(event,data){
                     
                     $.ajax({
                            url: '<%= Url.Action("Rename","PageManagerTree")%>',
                            type: 'POST',
                            timeout: "100000",
                            data: {
                                "id": data.rslt.obj.attr("id").replace("pjson_", ""),
                                "name": data.rslt.new_name
                            },
                            success: function (data) {
                                if (data.error == true) {

                                } else {
                                    $("#jsTree").jstree("refresh", -1);
                                }
                            }
                        });



                  }); 

    });

    function seeChanges(){
        window.location.href = window.location.href;
    }

    function AddNewPage(){
        $.ajax({
                url: '<%= Url.Action("create","PageManagerTree")%>',
                type: 'POST',
                timeout: "100000",
                data: {
                    "id": 0
                },
                success: function (data) {
                    $("#jsTree").jstree("refresh", -1);
                }
            });
    }

    function deletePage(id){
        if(id != 0) {           
       
            $.ajax({
                    url: '<%= Url.Action("remove","PageManagerTree")%>',
                    type: 'POST',
                    timeout: "100000",
                    data: {
                        "id": id
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
    }

    

</script>

