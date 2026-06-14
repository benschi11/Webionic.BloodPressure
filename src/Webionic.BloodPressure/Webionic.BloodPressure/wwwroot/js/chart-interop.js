window.chartInterop = {
    createLineChart: function (canvasId, labels, datasets, markerHighlights) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) return;

        // Destroy existing chart if any
        if (canvas._chartInstance) {
            canvas._chartInstance.destroy();
        }

        const markerPlugin = {
            id: 'timelineMarkers',
            beforeDatasetsDraw(chart, args, pluginOptions) {
                const highlights = pluginOptions?.highlights || [];
                if (!highlights.length) return;

                const { ctx, chartArea, scales } = chart;
                const xScale = scales.x;
                if (!xScale || !chartArea) return;

                ctx.save();
                highlights.forEach((highlight) => {
                    const x = xScale.getPixelForValue(highlight.index);
                    if (!Number.isFinite(x)) return;

                    const stripWidth = 14;
                    ctx.fillStyle = 'rgba(233, 196, 106, 0.18)';
                    ctx.fillRect(x - stripWidth / 2, chartArea.top, stripWidth, chartArea.bottom - chartArea.top);

                    ctx.strokeStyle = 'rgba(196, 132, 32, 0.45)';
                    ctx.lineWidth = 1;
                    ctx.beginPath();
                    ctx.moveTo(x, chartArea.top);
                    ctx.lineTo(x, chartArea.bottom);
                    ctx.stroke();
                });
                ctx.restore();
            }
        };

        const markerByIndex = new Map((markerHighlights || []).map(marker => [marker.index, marker]));

        const ctx = canvas.getContext('2d');
        canvas._chartInstance = new Chart(ctx, {
            type: 'line',
            plugins: [markerPlugin],
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
                        cornerRadius: 8,
                        callbacks: {
                            afterBody: (items) => {
                                if (!items || !items.length) return [];
                                const marker = markerByIndex.get(items[0].dataIndex);
                                if (!marker) return [];
                                return [`Marke: ${marker.title}`, `Zeitpunkt: ${marker.localTimestamp}`];
                            }
                        }
                    },
                    timelineMarkers: {
                        highlights: markerHighlights || []
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
