(function(){
CKEDITOR.plugins.add('accordion',
{
    init: function (editor) {
        var pluginName = 'accordion';
        editor.ui.addButton('Accordion',
            {
                label: 'Accordion',
                command: 'Accordion',
                icon: CKEDITOR.plugins.getPath('accordion') + 'accordion.png'
            });
        var cmd = editor.addCommand('Accordion', { exec: showMyDialog });
    }
});
function showMyDialog(e) {
    var textarea = CKEDITOR.instances.Content_ctl01.getData();
    var exist = textarea.toString().indexOf('class="accordion"');
    if (exist != -1) {
        alert('Already exists an accordion in this module.');
    } else {

        e.insertHtml(
            '<div class="accordion" id="hola">' +
                '<div class="accordion-head">Section 1</div>' +
                '<div class="accordion-body">' +
                '<p>Mauris mauris ante, blandit et, ultrices a, suscipit eget.Integer ut neque. Vivamus nisi metus, molestie vel, gravida in,condimentum sit amet, nunc. Nam a nibh. Donec suscipit eros.Nam mi. Proin viverra leo ut odio.</p>' +
                '</div>' +
                '<div class="accordion-head">Section 2</div>' +
                '<div class="accordion-body"> ' +
                '<p>Nam enim risus, molestie et, porta ac, aliquam ac, risus.Quisque lobortis.Phasellus pellentesque purus in massa.</p>' +
                '<ul>' +
                '<li>List item one</li>' +
                '<li>List item two</li>' +
                '<li>List item three</li>' +
                '</ul>' +
                '</div>' +
                '<div class="accordion-head">Section 3</div>' +
                '<div class="accordion-body">' +
                '<p>Sed non urna. Phasellus eu ligula. Vestibulum sit amet purus.Vivamus hendrerit, dolor aliquet laoreet, mauris turpis velit,faucibus interdum tellus libero ac justo.</p>' +
                '</div></br></br></div>' +
                '<script>' +
                '$(".accordion").accordion();' +
                '</script>'
        );
    }
}
})();