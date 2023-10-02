$('#activate-customer-button').click(function () {
    var customerId = $(this).data('customer-id');

    $.ajax({
        url: `/clientes/ativar/${customerId}`,
        type: 'PATCH',
        success: function (result) {
            if (result.success) {
                alert('Cliente ativado com sucesso.');
                location.reload();
            } else {
                alert('Falha ao ativar o cliente: ' + result.message);
            }
        },
        error: function () {
            alert('Ocorreu um erro ao ativar o cliente.');
        }
    });
});