tinymce.init({
    selector: "textarea",
    menubar: false,
    statusbar: true,
    resize: true,
    language: "ru",
    content_css: "/Content/css/editor.css",
    style_formats: [
        { title: 'Header 1', block: 'h3' },
        { title: 'Header 2', block: 'h4' },
        { title: 'Header 3', block: 'h5' }
    ]
});
