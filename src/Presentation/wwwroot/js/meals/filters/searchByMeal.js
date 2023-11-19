$(document).ready(function () {
    $("#search-meal-filter").on("keyup", function () {
        const searchTerm = $(this).val().toLowerCase();

        $("#meals-tableBody tr").each(function () {
            const mealName = $(this).find("td:eq(1)").text().toLowerCase();
            const accompaniments = $(this).find("td:eq(2)").text().toLowerCase();

            if (mealName.includes(searchTerm) || mealName.includes(accompaniments)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});