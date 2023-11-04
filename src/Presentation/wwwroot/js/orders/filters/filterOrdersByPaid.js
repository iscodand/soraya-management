$(document).ready(function () {
    // Bind an input event listener to the payment filter input
    $("#payment-filter").on("change", function () {
        const paymentFilter = $("#payment-filter").val();

        // Initialize a variable to keep track of the visible row count
        let visibleRowCount = 0;

        // Loop through each row in the table body
        $("#orders-tableBody tr").each(function () {
            const paidColumn = $(this).find("td:nth-child(7)");

            // Check if the payment status matches the selected filter
            const isPaid = paidColumn.find(".status.paid").length > 0;

            // Determine whether to show or hide the row based on the payment status filter
            if ((paymentFilter === "all") ||
                (paymentFilter === "paid" && isPaid) ||
                (paymentFilter === "unpaid" && !isPaid)) {
                $(this).show();
                visibleRowCount++;
            } else {
                $(this).hide();
            }
        });
    });
});