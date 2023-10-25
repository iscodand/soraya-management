export function fetchData(selectedDate) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: `data?dateRangeSelected=${selectedDate}`,
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