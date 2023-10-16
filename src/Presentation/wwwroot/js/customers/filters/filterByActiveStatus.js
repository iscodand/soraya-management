$(document).ready(function () {
    // Bind an input event listener to the is active filter input
    $("#isActive-filter").on("change", function () {
        const isActiveFilter = $("#isActive-filter").val();

        // Loop through each row in the table body
        $("#customers-tableBody tr").each(function () {
            const isActiveColumn = $(this).find("td:nth-child(4)");

            // Check if the isActive status matches the selected filter
            const isActive = isActiveColumn.find("#isActive-icon").length > 0;

            // Determine whether to show or hide the row based on the isActive status filter
            if ((isActiveFilter === "all") ||
                (isActiveFilter === "actives" && isActive) ||
                (isActiveFilter === "inactives" && !isActive)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});