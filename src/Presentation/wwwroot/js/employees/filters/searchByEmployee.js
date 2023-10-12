$(document).ready(function () {
    $("#search-employee-filter").on("keyup", function () {
        var searchTerm = $(this).val().toLowerCase();

        $("#employees-tableBody tr").each(function () {
            var employeeName = $(this).find("td:eq(2)").text().toLowerCase();

            if (employeeName.includes(searchTerm)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});