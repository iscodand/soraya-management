$('#deactivate-employee-button').click(function () {
    var employeeUsername = $(this).data('employee-username');

    $.ajax({
        url: `/minha-empresa/funcionarios/desativar/${employeeUsername}`,
        type: 'PATCH',
        success: function (result) {
            if (result.success) {
                alert('Usuário desativado com sucesso.');
                location.reload();
            } else {
                alert('Falha ao desativar o usuário: ' + result.message);
            }
        },
        error: function () {
            alert('Ocorreu um erro ao desativar o usuário.');
        }
    });
});