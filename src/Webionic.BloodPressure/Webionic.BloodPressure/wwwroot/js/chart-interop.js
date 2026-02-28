window.chartInterop = {
    createLineChart: function (canvasId, labels, datasets) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        // Destroy existing chart if any
        if (canvas._chartInstance) {
            canvas._chartInstance.destroy();
        }

        const ctx = canvas.getContext('2d');
        canvas._chartInstance = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: datasets.map(ds => ({
                    label: ds.label,
                    data: ds.data,
                    borderColor: ds.color,
                    backgroundColor: ds.color + '20',
                    borderWidth: 2.5,
                    pointRadius: 3,
                    pointHoverRadius: 6,
                    pointBackgroundColor: ds.color,
                    tension: 0.3,
                    fill: ds.fill || false
                }))
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: {
                    mode: 'index',
                    intersect: false,
                },
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            padding: 16,
                            font: { size: 13 }
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(0,0,0,0.8)',
                        titleFont: { size: 13 },
                        bodyFont: { size: 12 },
                        padding: 12,
                        cornerRadius: 8
                    }
                },
                scales: {
                    x: {
                        grid: { display: false },
                        ticks: {
                            maxRotation: 45,
                            font: { size: 11 }
                        }
                    },
                    y: {
                        beginAtZero: false,
                        grid: { color: 'rgba(0,0,0,0.06)' },
                        ticks: { font: { size: 11 } }
                    }
                }
            }
        });
    },

    destroyChart: function (canvasId) {
        const canvas = document.getElementById(canvasId);
        if (canvas && canvas._chartInstance) {
            canvas._chartInstance.destroy();
            canvas._chartInstance = null;
        }
    }
};
