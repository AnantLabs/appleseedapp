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
    e.insertHtml(
        '<div id="accordion">' +
            '<h3>Section 1</h3>' +
            '<div>' +
                '<p>Mauris mauris ante, blandit et, ultrices a, suscipit eget.Integer ut neque. Vivamus nisi metus, molestie vel, gravida in,condimentum sit amet, nunc. Nam a nibh. Donec suscipit eros.Nam mi. Proin viverra leo ut odio.</p>' +
            '</div>' +
            '<h3>Section 2</h3>' +
            '<div>' +
                '<p>Nam enim risus, molestie et, porta ac, aliquam ac, risus.Quisque lobortis.Phasellus pellentesque purus in massa.</p>' +
                '<ul>' +
                    '<li>List item one</li>' +
                    '<li>List item two</li>' +
                    '<li>List item three</li>' +
                '</ul>' +
            '</div>' +
            '<h3>Section 3</h3>' +
            '<div>' +
                'Sed non urna. Phasellus eu ligula. Vestibulum sit amet purus.Vivamus hendrerit, dolor aliquet laoreet, mauris turpis velit,faucibus interdum tellus libero ac justo.<p>' +
            '</div>' +
        '</div>' +
        '<script>' +
            '$("#accordion").accordion();' +
            'var newid =  document.getElementById("accordion").parentNode.parentNode.parentNode.parentNode.parentNode.id;'+
            'document.getElementById("accordion").id = "accordion_" + newid;' +
        '</script>'
    );
}
})();