export function createOrUpdateBarChart(ctx, chart, datasetLabel, labels, data, title) {
    if (chart) {
        chart.destroy();
        chart = null;
    }

    return new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: datasetLabel,
                data: data,
                borderWidth: 2,
                backgroundColor: '#4d5acba9',
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
