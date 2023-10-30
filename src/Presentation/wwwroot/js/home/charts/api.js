export function fetchData(selectedDate, initialDate, finalDate) {
    let baseUrl = [];
    baseUrl.push(`data`)

    if (!initialDate || !finalDate) {
        baseUrl.push(`?selectedDate=${selectedDate}`)
    } else {
        baseUrl.push(`?initialDate=${initialDate}&finalDate=${finalDate}`)
    }

    return new Promise((resolve, reject) => {
        $.ajax({
            url: baseUrl.join(""),
            type: 'GET',
            success: (result) => {
                if (result.success) {
                    resolve(result.data);
                } else {
                    reject('Ocorreu um erro ao recuperar os dados.');
                }
            },
            error: () => {
                reject('Ocorreu um erro ao recuperar os dados.');
            }
        });
    });
}