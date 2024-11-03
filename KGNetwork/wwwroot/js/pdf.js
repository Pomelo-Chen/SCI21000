function loadPdf(url) {
    var pdfContainer = document.getElementById('pdf-container');
    pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.6.347/pdf.worker.min.js';

    pdfjsLib.getDocument(url).promise.then(function (pdfDoc) {
        var pageNum = 1;
        renderPage(pdfDoc, pageNum);
    });

    function renderPage(pdfDoc, pageNum) {
        pdfDoc.getPage(pageNum).then(function (page) {
            var scale = 1.5;
            var viewport = page.getViewport({ scale: scale });
            var canvas = document.createElement('canvas');
            var context = canvas.getContext('2d');
            canvas.height = viewport.height;
            canvas.width = viewport.width;

            var renderContext = {
                canvasContext: context,
                viewport: viewport
            };

            page.render(renderContext).promise.then(function () {
                pdfContainer.appendChild(canvas);
            });
        });
    }
}