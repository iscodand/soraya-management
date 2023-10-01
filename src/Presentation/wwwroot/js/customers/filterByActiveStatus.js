$(document).ready(function () {
    // Bind an input event listener to the payment filter input
    $("#payment-filter").on("change", function () {
        const paymentFilter = $("#payment-filter").val();

        // Loop through each row in the table body
        $("#orders-tableBody tr").each(function () {
            const paidColumn = $(this).find("td:nth-child(7)");

            // Check if the payment status matches the selected filter
            const isPaid = paidColumn.find("i#isPaid-icon").length > 0;

            // Determine whether to show or hide the row based on the payment status filter
            if ((paymentFilter === "all") ||
                (paymentFilter === "paid" && isPaid) ||
                (paymentFilter === "unpaid" && !isPaid)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});