

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
        "<h3>New panel</h3>" +
         "<div>New data<p>" +
         "</div>"   
    );
}

