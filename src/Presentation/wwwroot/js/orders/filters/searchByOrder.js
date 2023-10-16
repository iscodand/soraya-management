$(document).ready(function () {
    $("#search-order-filter").on("keyup", function () {
        let searchTerm = $(this).val().toLowerCase();
        let visibleRowCount = 0;

        $("#orders-tableBody tr").each(function () {
            let customerName = $(this).find("td:eq(2)").text().toLowerCase();
            let mealName = $(this).find("td:eq(3)").text().toLowerCase();

            if (customerName.includes(searchTerm) || mealName.includes(searchTerm)) {
                $(this).addClass("fade-in").removeClass("fade-out");
                visibleRowCount++;
            } else {
                $(this).addClass("fade-out").removeClass("fade-in");
            }
        });

        const caption = $(".styled-table-caption");
        caption.text(`Pedidos Encontrados: ${visibleRowCount}`);
    });
});