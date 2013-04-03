<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="PageManagerTree" %>

<div id="jqtreePageManagement">
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
                    "url":
                        function (node) {
                            //var nodeId = node.attr('id'); 
                            if (node == -1) {
                                return '<%= Url.Action("GetTreeData", "PageManagerTree") %>';
                            }else
                                return '<%= Url.Action("AddModule", "PageManagerTree") %>' + '?id=' + node.attr('id');
                        },
                    
                    "data": function (n) {
                        return { ID: n.attr ? n.attr("ID") : 0 };
                    },
                    "success": function (new_data) {
                        return new_data;
                    }
                }
            },
            "types": {
                "max_depth" : -2,
                "max_children" : -2,
                "valid_children": "none",
                "types": {
                    "file": {
                        "valid_children": "none",
                        "icon": {
                            "image": "/images/file.png",
                        }
                    },
                    "folder": {
                        "valid_children": ["file"],
                        "icon": {
                            "image": "/images/folder.png",
                        },
                        //"start_drag" : false,
					    //"move_node" : false,

                        //"delete_node": false,
					    //"remove": false,
                        "rename": false,
                    },
                    "root": {
                        "valid_children": "Default",
                        "icon": {
                            "image": "/images/root.png",
                        },
                        "start_drag" : false,
                        "move_node" : false,
                        "delete_node" : false,
                        "remove" : false
                    }
                },
            },
            "contextmenu": {
                "items": {
                    ViewItem: {
                        label: '<%: Appleseed.Framework.General.GetString("View","View") %>',
                        action: function (obj) {
                            var currentId = this._get_node(obj).attr("id");
                            $.ajax({
                                url: '<%= Url.Action("ViewPage")%>',
                                type: 'POST',
                                timeout: "100000",
                                data: {
                                    "pageId": currentId.replace("pjson_", "")
                                },
                                success: function (data) {
                                    window.location.href = data;
                                }
                                
                            });

                        },
                    },
                    renameItem: {
                        "label": '<%: Appleseed.Framework.General.GetString("RENAME") %>',
                        "action": function (obj) {
                            this.rename(obj);

                        },
                    },
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
                    "rename": false,
                    

                    
                    cloneItem: {
                        label: '<%: Appleseed.Framework.General.GetString("Clone","Clone") %>',
                        action: function (obj) {
                            var currentId = this._get_node(obj).attr("id");
                            var parentId = this._get_node(obj)[0].firstChild.parentElement.parentNode.parentElement.id;
                            $.ajax({
                                url: '<%= Url.Action("Clone")%>',
                                type: 'POST',
                                timeout: "100000",
                                data: {
                                    "id": currentId.replace("pjson_", ""),
                                    "parentId": parentId.replace("pjson_", "")
                                },
                                success: function (data) {
                                    var name = $("#jsTree").jstree("get_text", '#' + currentId) + ' - Clone';
                                    $("#jsTree").jstree("create", "#" + parentId, "last", { 'attr': { 'id': 'pjson_' + data.pageId }, 'title': name }, false, true);
                                    $("#jsTree").jstree("set_text", '#pjson_' + data.pageId, name);
                                    $("#jsTree").jstree("rename", "#pjson_" + data.pageId);
                                },
                                error: function (data) {
                                    $.jstree.rollback(obj.rlbk);
                                }                                    
                            });
                            
                        },
                    },
                    copyItem: {
                        label: '<%: Appleseed.Framework.General.GetString("COPY", "Copy") %>',
                        action: function (obj) {
                            var currentId = this._get_node(obj).attr("id");
                            var currentName = this._get_node(obj).text().replace(/\s{2}/, "");
                            var children = this._get_node(obj).children().children().text().replace(/\s/, "");
                            var folder = currentName.replace(children, "");
                            var parentId = this._get_node(obj)[0].firstChild.parentElement.parentNode.parentElement.id;
                            $.ajax({
                                url: '<%= Url.Action("CopyPage")%>',
                                type: 'POST',
                                timeout: "100000",
                                data: {
                                    "pageId": currentId.replace("pjson_", ""),
                                    "name": folder,
                                },
                                success: function (data) {
                                    var name = $("#jsTree").jstree("get_text", '#' + currentId) + ' - Copy';
                                    $("#jsTree").jstree("create", "#" + parentId, "last", { 'attr': { 'id': 'pjson_' + data.pageId }, 'title': name }, false, true);
                                    $("#jsTree").jstree("set_text", '#pjson_' + data.pageId, name);
                                    $("#jsTree").jstree("rename", "#pjson_" + data.pageId);
                                },
                                error: function (data) {
                                    $.jstree.rollback(obj.rlbk);
                                }
                            });

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

                                
                                
                            },
                        "separator_before": true
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

            "plugins": ["themes", "contextmenu", "json_data", "ui", "crrm", "dnd", "core", "types"]
        })
        .bind("move_node.jstree", function (e, data) {
            var selectedId = data.rslt.o.attr("id");
            
            if (selectedId.indexOf("module") != -1) {
                var selected = data.rslt.np.text().replace(/\s{2}/, "");
                var children = data.rslt.np.children().children().text().replace(/\s/, "");
                var folder = selected.replace(children, "");
                $.ajax({
                    url: '<%= Url.Action("MoveModuleNode","PageManagerTree")%>',
                    type: 'POST',
                    timeout: "100000",
                    data: {
                        "pageId": $.jstree._focused()._get_parent(data.rslt.np).attr("id").replace("pjson_", ""),
                        "paneName": folder,
                        "moduleId": data.rslt.o.attr("id").replace("pjson_module_", "")
                    },
                    success: function (data) {
                    }
                });
            } else {
                if ((typeof(data.rslt.or.attr("id"))) == 'undefined') {
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
                    success: function(data) {
                    }
                });
            }
        })
        .bind("rename.jstree",function(event,data) {
            var selectedId = data.rslt.obj.attr("id");
            if (selectedId.indexOf("module") != -1) {
                $.ajax({
                    url: '<%= Url.Action("RenameModule","PageManagerTree")%>',
                    type: 'POST',
                    timeout: "100000",
                    data: {
                        "id": data.rslt.obj.attr("id").replace("pjson_module_", ""),
                        "name": data.rslt.new_name
                    },
                    success: function (data) {
                        if (data.error == true) {

                        } else {
                            $("#jsTree").jstree("refresh", -1);
                        }
                    }
                });
            } else {

                $.ajax({
                    url: '<%= Url.Action("Rename","PageManagerTree")%>',
                    type: 'POST',
                    timeout: "100000",
                    data: {
                        "id": data.rslt.obj.attr("id").replace("pjson_", ""),
                        "name": data.rslt.new_name
                    },
                    success: function(data) {
                        if (data.error == true) {

                        } else {
                            $("#jsTree").jstree("refresh", -1);
                        }
                    }
                });
            }
        })
        .bind("open_node.jstree", function (event, data) {
            var  pageid = data.rslt.obj.attr("id").replace("pjson_", "");
            if ((pageid != 0) && (pageid.indexOf('_') == -1)) {
                $.ajax({
                    url: '<%= Url.Action("AddModule","PageManagerTree")%>',
                    type: 'POST',
                    data: {
                        "id": pageid.replace("pjson_", ""),
                    },
                    success: function (data) {
                        return data;
                    }
                });
            }

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

    $(function() {
         var c;
        $('#jqtreePageManagement').children().each(function (i) {
            var vs = this;
            if($(vs).has('input').length > 0){
                c = $(vs).children(0); 
                if($(c).attr('id') == '__VIEWSTATE')  {        
                    $(c).remove();
                    $(vs).remove();
                } 
            }      
        });
    });

    function logNop() {
        console.log('');
  
    }

</script>

</div>