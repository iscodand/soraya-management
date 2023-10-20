$('#delete-meal-button').click(function () {
    var customerId = $(this).data('meal-id');

    if (confirm('Tem certeza de que deseja deletar este sabor?.')) {
        $.ajax({
            url: `/sabores/deletar/${customerId}`,
            type: 'DELETE',
            success: function (result) {
                if (result.success) {
                    alert('Sabor deletado com sucesso.');
                    location.href = '/sabores';
                } else {
                    alert('Falha ao deletar o sabor: ' + result.message);
                }
            },
            error: function () {
                alert('Ocorreu um erro ao deletar o sabor.');
            }
        });
    }
});