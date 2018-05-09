$(function () {
    //Wait for DOM to be loaded (ready function)
    
    //DataTables
    $("#tablesorted").DataTable({
        "columnDefs": [
            {"orderable":false,"targets":-1},//stop sorting on last column
        ],
        "lengthMenu":[ [5,10,25,50,-1], [5,10,25,50,"All"] ]//Drop down for how many entries
    });

    //DataTables: alterative - without paging (used for instructors)
    $("#tablesorted-alt").DataTable({
        "columnDefs": [
            { "orderable": false, "targets": -1 },//stop sorting on last column
        ],
        "paging":false,       
    });
    /**
     * Store scroll position for and set it after reload
     *
     * @return {boolean} [localStorage is available]
     */
    $.fn.scrollPosReload = function () {
        if (localStorage) {
            var posReader = localStorage["posStorage"];
            if (posReader) {
                //console.log(posReader);
                //$(window).scrollTop(posReader);
                $('html, body').animate({ scrollTop: posReader }, 600);
                localStorage.removeItem("posStorage");
            }
            $(this).click(function (e) {
                //alert('click: ' + $(window).scrollTop());
                localStorage["posStorage"] = $(window).scrollTop();
            });

            return true;
        }

        return false;
    }

    //Instructors Page
    $('.scroll').scrollPosReload();

});
