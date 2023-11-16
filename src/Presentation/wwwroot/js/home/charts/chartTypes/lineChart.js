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
                pointStyle: 'circle',
                pointRadius: 3,
                pointHoverRadius: 15,
                fill: true,
                backgroundColor: '#4d5acba9',
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return `R$ ${value}.00`
                        }
                    }
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
