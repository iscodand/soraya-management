export function createOrUpdateLineChart(ctx, chart, datasetLabel, labels, data, title) {
    if (chart) {
        chart.destroy();
        chart = null;
    }

    return new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: datasetLabel,
                data: data,
                borderWidth: 2,
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: title,
                },
            },
        },
    });
}
