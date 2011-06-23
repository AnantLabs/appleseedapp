function EditTitleInLine(url){
	
	$(".editTitle").editable(submitEdit, { 
				
				indicator : "Saving...",
				tooltip   : "Click to edit...",
				name : "Editable.FieldName",
				id   : "elementid",
				type : "text",
				cssclass : 'NormalTextBox valid',

	});
	
	function submitEdit(value, settings)
	{ 
	   var edits = new Object();
	   var origvalue = this.revert;
	   var textbox = this;
	   var result = value;
	   // var pagina = data;
	   edits[settings.name] = [value];
	   var returned = $.ajax({
			   url: url, 
			   type: "POST",
			   data: "id="+textbox.id+"&value="+result,
			   });
	   return(result);
	 }
	
}

