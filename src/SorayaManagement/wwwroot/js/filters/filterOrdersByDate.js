$(document).ready(function () {
    $('#filterByDate-form').on('change', function (e) {
        e.preventDefault();

        let formData = $(this).serialize();

        $.ajax({
            url: '/pedidos/filtering',
            type: 'GET',
            data: formData,
            success: function (result) {
                $('#orders-tableBody').html(result);

                let $orders = $('#orders-tableBody tr');
                let newOrdersCount = $orders.length;
                let paidOrdersCount = $orders.filter(':has(.fa-square-check)').length;
                let unpaidOrdersCount = newOrdersCount - paidOrdersCount;

                $('#new-orders-count').text(newOrdersCount);
                $('#paid-orders-count').text(paidOrdersCount);
                $('#unpaid-orders-count').text(unpaidOrdersCount);

                let totalRevenue = 0;

                $orders.each(function () {
                    let priceText = $(this).find('td:eq(4)').text().replace('R$', '').trim();
                    let price = parseFloat(priceText.replace(',', '.'));

                    if (!isNaN(price)) {
                        totalRevenue += price;
                    }
                });

                let formattedTotalRevenue = 'R$ ' + totalRevenue.toFixed(2);
                $('#price-orders-sum').text(formattedTotalRevenue);
            },
            error: function () {
                alert('Ocorreu um erro ao processar a solicitação.');
            }
        });
    });
});