window.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("order-details-modal");
    const modalCard = document.getElementById("order-details-modal-card");
    const modalBackdrop = document.getElementById("modal-backdrop");

    function closeModal() {
        if (modal) {
            modal.classList.add('hidden');
        }

        modalCard.classList.add('hidden');
        modalBackdrop.classList.add('hidden');
    }

    if (modalBackdrop) {
        modalBackdrop.addEventListener('click', () => {
            closeModal();
        });
    }
})