

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
    var textarea = CKEDITOR.instances.Content_ctl01.getData();
    var exist = textarea.toString().indexOf('class="accordion"');
    if (exist == -1) {
        alert('Not exists an accordion in this module.');
    } else {
        e.insertHtml(
            '<div class="accordion-head" >New panel</div>' +
                '<div class="accordion-body"> ' +
                '<p> New data.</p>' +
                '</div></br>'
        );
    }
}

