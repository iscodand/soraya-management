$('#activate-employee-button').click(function () {
    var employeeUsername = $(this).data('employee-username');

    $.ajax({
        url: `/minha-empresa/funcionarios/ativar/${employeeUsername}`,
        type: 'PATCH',
        success: function (result) {
            if (result.success) {
                alert('Usuário ativado com sucesso.');
                location.reload();
            } else {
                alert('Falha ao ativar o usuário: ' + result.message);
            }
        },
        error: function () {
            alert('Ocorreu um erro ao ativar o usuário.');
        }
    });
});