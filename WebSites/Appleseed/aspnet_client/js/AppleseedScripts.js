function EditTitleInLine(url){
	
	$(".editTitle").editable(submitEdit, { 
				
				indicator : "Saving...",
				tooltip   : "Click to edit...",
				name : "Editable.FieldName",
				event     : "dblclick",
				id   : "elementid",
				cancel : 'Cancel',
         		submit : 'OK',
				type : "text",
				cssclass : 'CommandButton'

	});
	
	function submitEdit(value, settings)
	{ 
	   var edits = new Object();
	   var origvalue = this.revert;
	   var textbox = this;
	   var result = value;
	   // var pagina = data;
	   edits[settings.name] = [value];
	   var succ = origvalue;
	   var returned = $.ajax({
			   url: url, 
			   type: "POST",
			   async: false,
			   data: {
					id: textbox.id,
					value: result
			   },
			   success : function(data){
					
					if(data.result == true){
						succ = result;
						
					}
					else{
						succ = origvalue;
						
					};
					
			   }
			   });
	   return(succ);
	 }
	
}

function setDialog(id, dialogwidth, dialogheigth) {
	var divDialog = "HtmlModuleDialog" + id;
    var iframe = "HtmlMoudleIframe" + id;
	$('.' + iframe).ready(function() {
			$('.' + divDialog).dialog({
				width: dialogwidth,
				height: dialogheigth,
				resizable : false,
				autoOpen: false
			});
		});
}

function editHtml(id, pageID, dir) {

        var divDialog = "HtmlModuleDialog" + id;
        var iframe = "HtmlMoudleIframe" + id;
		var url = dir+"&pageID"+pageID;
        $('.' + iframe).attr('src', url );
		$('.' + iframe).ready(function() {
			$('.' + divDialog).dialog("open");
		});


    }
	
function openInModal(dir,modalTitle){
	$('#iframemodal').remove();
	$('<div id="iframemodal">').html("<iframe src='"+dir+"&ModalChangeMaster=true' width='100%' height='99%' > </iframe>").dialog({
		width: 1050,
		height: 600,
		title: modalTitle,
		resizable: false,
		minWidth: 800,
		minHeight: 600,
        modal: true
	});
	return false;
}

function ChangeModalTitle(modalTitle){
	$('#iframemodal').dialog('option', 'title', modalTitle);

}


$(function () {
    $("#accordionPageLayout").accordion({ autoHeight: false });
});                
            