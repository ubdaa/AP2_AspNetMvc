function InitChart(data, labels, label, chartName, chartType, borderColor, backgroundColor) {
    var ctx = document.getElementById(chartName).getContext('2d');
    var chart = new Chart(ctx, {
        type: chartType,
        data: {
            labels: labels,
            datasets: [{
                label: label,
                data: data,
                borderColor: borderColor,
                backgroundColor: backgroundColor,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });

    return chart;
}