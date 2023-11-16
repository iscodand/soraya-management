$('#mark-as-paid-button').click(function () {
    var orderId = $(this).data('order-id');
    var pendingFlag = document.getElementsByClassName('pending');

    $.ajax({
        url: `/pedidos/marcar-como-pago/${orderId}`,
        type: 'PATCH',
        success: function (result) {
            if (result.success) {
                for (var i = 0; i < pendingFlag.length; i++) {
                    pendingFlag[i].classList.remove('pending');
                    pendingFlag[i].classList.add('paid');
                }
            } else {
                alert('Falha ao atualizar o pedido: ' + result.message);
            }
        },
        error: function (xhr) {
            console.error(xhr.responseText);
        }
    });
});