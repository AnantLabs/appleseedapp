(function(){
CKEDITOR.plugins.add('accordion',
{
    init: function (editor) {
        var pluginName = 'accordion';
        editor.ui.addButton('Accordion',
            {
                label: 'Accordion',
                command: 'Accordion',
                icon: CKEDITOR.plugins.getPath('accordion') + 'accordion.png',
            });
        var cmd = editor.addCommand('Accordion', { exec: showMyDialog });
    }
});
function showMyDialog(e) {
    var textarea = CKEDITOR.instances.Content_ctl01.getData();
    var exist = textarea.toString().indexOf('class="accordion"');
    if (exist != -1) {
        var newitem = textarea.toString().replace('<!--ITEM-->', '<div class="accordion-head">This is where the accordion header goes.</div>' +
                '<div class="accordion-body">' +
                    '<p>This is where the accordion body goes.</p>' +
                '</div><!--ITEM-->');
        CKEDITOR.instances.Content_ctl01.setData(newitem);
    } else {

        e.insertHtml(
            '<div class="accordion">' +
                '<div class="accordion-head">This is where the accordion header goes.</div>' +
                '<div class="accordion-body">' +
                    '<p>This is where the accordion body goes.</p>' +
                '</div><!--ITEM--><br><br>' +
            '</div>' +
            '<script>' +
            '$(".accordion").accordion({'+
                'collapsible: true,' +
                'active: false, ' +
                'alwaysOpen: false, ' +
            '});'+
            '</script>'
        );
    }
}
})();