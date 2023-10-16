function formatPhoneNumber(inputElementId) {
    const phoneInput = document.getElementById(inputElementId);

    if (!phoneInput) {
        console.error(`Element with ID ${inputElementId} not found.`);
        return;
    }

    phoneInput.addEventListener('input', function () {
        let phoneNumber = phoneInput.value;
        phoneNumber = phoneNumber.replace(/\D/g, '');

        if (phoneNumber.length >= 11) {
            phoneNumber = `(${phoneNumber.substring(0, 2)}) ${phoneNumber.substring(2, 3)} ${phoneNumber.substring(3, 7)}-${phoneNumber.substring(7)}`;
        } else if (phoneNumber.length >= 3) {
            phoneNumber = `(${phoneNumber.substring(0, 2)}) ${phoneNumber.substring(2, 3)} ${phoneNumber.substring(3)}`;
        }

        phoneInput.value = phoneNumber;
    });
}

formatPhoneNumber('customerPhoneInput');