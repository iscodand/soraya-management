$('#mark-as-paid-button').click(function () {
    var orderId = $(this).data('order-id');
    var pendingFlag = document.getElementsByClassName('pending');
    var modal = document.getElementById('order-details-modal')
    var modalBackdrop = document.getElementById('modal-backdrop');

    $.ajax({
        url: `/pedidos/marcar-como-pago/${orderId}`,
        type: 'PATCH',
        success: function (result) {
            console.log()
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