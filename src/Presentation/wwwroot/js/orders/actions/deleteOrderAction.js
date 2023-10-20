$('#delete-order-button').click(function () {
    var orderId = $(this).data('order-id');

    if (confirm('Tem certeza de que deseja excluir este pedido?')) {
        $.ajax({
            url: `/pedidos/deletar/${orderId}`,
            type: 'DELETE',
            success: function (result) {
                if (result.success) {
                    alert('Pedido excluído com sucesso.');
                    location.reload();
                } else {
                    alert('Falha ao excluir o pedido: ' + result.message);
                }
            },
            error: function () {
                alert('Ocorreu um erro ao excluir o pedido.');
            }
        });
    }
});