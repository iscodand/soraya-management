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

                let totalReceived = 0;
                let totalToReceive = 0;
                let paidOrdersCount = 0;
                let unpaidOrdersCount = 0;

                $('#orders-tableBody tr').each(function () {
                    let isPaid = $(this).find('.status.paid').length > 0;
                    let totalReceivedField = $(this).find('td:eq(4)').text().replace('R$', '').trim();
                    let price = parseFloat(totalReceivedField);

                    if (!isNaN(price)) {
                        totalReceived += price;
                        if (isPaid) {
                            paidOrdersCount++;
                        } else {
                            unpaidOrdersCount++;
                            totalToReceive += price;
                        }
                    }
                });

                let formattedTotalReceived = 'R$ ' + totalReceived.toFixed(2);
                let formattedTotalToReceive = 'R$ ' + totalToReceive.toFixed(2);

                $('#new-orders-count').text(paidOrdersCount + unpaidOrdersCount);
                $('#price-orders-sum').text(formattedTotalReceived);
                $('#total-to-receive').text(formattedTotalToReceive);
                $('#paid-orders-count').text(paidOrdersCount);
                $('#unpaid-orders-count').text(unpaidOrdersCount);
            },
            error: function () {
                alert('Ocorreu um erro ao processar a solicitação.');
            }
        });
    });
});
