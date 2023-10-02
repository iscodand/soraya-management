$('#inactivate-customer-button').click(function () {
    var customerId = $(this).data('customer-id');

    if (confirm('Tem certeza de que deseja desativar este cliente? \n Os pedidos desse cliente não serão perdidos.')) {
        $.ajax({
            url: `/clientes/desativar/${customerId}`,
            type: 'PATCH',
            success: function (result) {
                if (result.success) {
                    alert('Cliente desativado com sucesso. Você pode reativá-lo a qualquer momento.');
                    location.reload();
                } else {
                    alert('Falha ao inativar o cliente: ' + result.message);
                }
            },
            error: function () {
                alert('Ocorreu um erro ao inativar o cliente.');
            }
        });
    }
});