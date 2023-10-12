$('#delete-customer-button').click(function () {
    var customerId = $(this).data('customer-id');

    if (confirm('Tem certeza de que deseja deletar este cliente?.')) {
        $.ajax({
            url: `/clientes/deletar/${customerId}`,
            type: 'DELETE',
            success: function (result) {
                if (result.success) {
                    alert('Cliente deletado com sucesso.');
                    location.href = '/clientes';
                } else {
                    alert('Falha ao deletar o cliente: ' + result.message);
                }
            },
            error: function () {
                alert('Ocorreu um erro ao deletar o cliente.');
            }
        });
    }
});