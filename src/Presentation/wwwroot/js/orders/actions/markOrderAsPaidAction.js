$('#mark-as-paid-button').click(function () {
    var orderId = $(this).data('order-id');

    $.ajax({
        url: `/pedidos/marcar-como-pago/${orderId}`,
        type: 'PATCH',
        success: function (result) {
            if (result.success) {
                location.reload();
            } else {
                alert('Falha ao atualizar o pedido: ' + result.message);
            }
        },
        error: function (xhr) {
            console.error(xhr.responseText);
        }
    });
});