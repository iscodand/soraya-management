$(document).ready(function () {
    $('.show-details').on('click', function (e) {
        e.preventDefault();

        const orderId = $(this).data('order-id');
        const orderDetailsCard = $('.order-details-modal-card');
        const backdrop = $('.modal-backdrop');

        $.ajax({
            url: `/pedidos/detalhes/${orderId}`,
            type: 'GET',
            success: function (data) {
                orderDetailsCard.html(data);
                orderDetailsCard.removeClass('hidden');
                backdrop.removeClass('hidden');
            },
            error: function () {
                alert('Ocorreu um erro ao carregar os detalhes da ordem.');
            }
        });
    });
});