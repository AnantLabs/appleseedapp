function EditTitleInLine(url){
	
	$(".editTitle").editable(submitEdit, { 
				
				indicator : "Saving...",
				tooltip   : "Click to edit...",
				name : "Editable.FieldName",
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
					
					if(data.result = 'true'){
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

