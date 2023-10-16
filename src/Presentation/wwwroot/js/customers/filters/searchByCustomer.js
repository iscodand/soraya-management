$(document).ready(function () {
    $("#search-customer-filter").on("keyup", function () {
        var searchTerm = $(this).val().toLowerCase();

        $("#customers-tableBody tr").each(function () {
            var customerName = $(this).find("td:eq(1)").text().toLowerCase();

            if (customerName.includes(searchTerm)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});