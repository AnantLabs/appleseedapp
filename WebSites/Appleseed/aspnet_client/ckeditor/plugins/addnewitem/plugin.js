

CKEDITOR.plugins.add('addnewitem',
{
    init: function (editor) {
        var pluginName = 'addnewitem';
        editor.ui.addButton('Addnewitem',
            {
                label: 'Add new item',
                command: 'Addnewitem',
                icon: CKEDITOR.plugins.getPath('addnewitem') + 'accordion.png'
            });
        var cmd = editor.addCommand('Addnewitem', { exec: showMyDialog });
    }
});
function showMyDialog(e) {
    e.insertHtml(
        '<div class="accordion-head" >New panel</div>' +
         '<div class="accordion-body"> ' +
            '<p> New data.</p>' +
         '</div></br>'   
    );
}

