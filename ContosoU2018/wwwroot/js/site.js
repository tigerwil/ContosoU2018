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
});
