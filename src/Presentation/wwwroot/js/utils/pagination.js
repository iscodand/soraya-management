$(document).ready(function () {
    var itemsPerPage = 5;
    var currentPage = 1;
    var totalItems = $('.styled-table .table-row').length;

    var totalPages = Math.ceil(totalItems / itemsPerPage);
    $('#totalPages').text(' de ' + totalPages);

    function showItems() {
        var rows = $('.styled-table .table-row');
        var startIndex = (currentPage - 1) * itemsPerPage;
        var endIndex = startIndex + itemsPerPage;

        rows.hide();
        rows.slice(startIndex, endIndex).show();

        $('#currentPage').text('Página ' + currentPage);
    }

    $('#nextPage').click(function () {
        if (currentPage < totalPages) {
            currentPage++;
            showItems();
        }
    });

    $('#previousPage').click(function () {
        if (currentPage > 1) {
            currentPage--;
            showItems();
        }
    });

    showItems();
});