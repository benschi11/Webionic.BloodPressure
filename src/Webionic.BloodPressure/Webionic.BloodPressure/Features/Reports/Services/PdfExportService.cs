using System.Globalization;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Webionic.BloodPressure.Features.BloodPressure.Models;
using Webionic.BloodPressure.Features.Reports.Models;

namespace Webionic.BloodPressure.Features.Reports.Services;

public class PdfExportService : IPdfExportService
{
    public byte[] GenerateReport(BloodPressureStats stats, List<BloodPressureReadingDto> readings, int days)
    {
        var sortedReadings = readings.OrderBy(r => r.Timestamp).ToList();
        var periodLabel = days switch
        {
            7 => "7 Tage",
            30 => "30 Tage",
            90 => "90 Tage",
            365 => "1 Jahr",
            _ => $"{days} Tage"
        };

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(40);
                page.MarginVertical(50);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text("Blutdruck-Report").FontSize(22).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().Text($"Zeitraum: {periodLabel} ({DateTime.Now.AddDays(-days):dd.MM.yyyy} – {DateTime.Now:dd.MM.yyyy})")
                        .FontSize(11).FontColor(Colors.Grey.Darken1);
                    col.Item().PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                });

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    // Statistics section
                    col.Item().Element(c => ComposeStats(c, stats));

                    // Blood pressure chart
                    if (sortedReadings.Count > 1)
                    {
                        col.Item().Element(c => ComposeChart(c, sortedReadings, "Blutdruckverlauf", "bloodpressure"));
                        col.Item().Element(c => ComposeChart(c, sortedReadings, "Pulsverlauf", "pulse"));
                    }

                    // Readings table
                    if (sortedReadings.Count > 0)
                    {
                        col.Item().Element(c => ComposeReadingsTable(c, sortedReadings));
                    }
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Seite ");
                    text.CurrentPageNumber();
                    text.Span(" von ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeStats(IContainer container, BloodPressureStats stats)
    {
        container.Column(col =>
        {
            col.Item().Text("Durchschnittswerte").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
            col.Item().PaddingTop(8).Row(row =>
            {
                row.RelativeItem().Background(Colors.Red.Lighten5).Padding(10).Column(inner =>
                {
                    inner.Item().AlignCenter().Text($"{stats.AverageSystolic:F0}").FontSize(20).Bold().FontColor(Colors.Red.Darken2);
                    inner.Item().AlignCenter().Text("Ø Systole (mmHg)").FontSize(8).FontColor(Colors.Grey.Darken1);
                    inner.Item().AlignCenter().PaddingTop(4).Text($"Min {stats.MinSystolic} / Max {stats.MaxSystolic}").FontSize(8).FontColor(Colors.Grey.Medium);
                });
                row.ConstantItem(8);
                row.RelativeItem().Background(Colors.Blue.Lighten5).Padding(10).Column(inner =>
                {
                    inner.Item().AlignCenter().Text($"{stats.AverageDiastolic:F0}").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                    inner.Item().AlignCenter().Text("Ø Diastole (mmHg)").FontSize(8).FontColor(Colors.Grey.Darken1);
                    inner.Item().AlignCenter().PaddingTop(4).Text($"Min {stats.MinDiastolic} / Max {stats.MaxDiastolic}").FontSize(8).FontColor(Colors.Grey.Medium);
                });
                row.ConstantItem(8);
                row.RelativeItem().Background(Colors.Teal.Lighten5).Padding(10).Column(inner =>
                {
                    inner.Item().AlignCenter().Text($"{stats.AveragePulse:F0}").FontSize(20).Bold().FontColor(Colors.Teal.Darken2);
                    inner.Item().AlignCenter().Text("Ø Puls (bpm)").FontSize(8).FontColor(Colors.Grey.Darken1);
                    inner.Item().AlignCenter().PaddingTop(4).Text($"Min {stats.MinPulse} / Max {stats.MaxPulse}").FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });
            col.Item().PaddingTop(4).AlignRight().Text($"{stats.TotalReadings} Messungen").FontSize(9).FontColor(Colors.Grey.Darken1);
        });
    }

    private static void ComposeChart(IContainer container, List<BloodPressureReadingDto> readings, string title, string chartType)
    {
        container.Column(col =>
        {
            col.Item().Text(title).FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
            col.Item().PaddingTop(5).Height(180).Svg(GenerateChartSvg(readings, chartType));
        });
    }

    private static string GenerateChartSvg(List<BloodPressureReadingDto> readings, string chartType)
    {
        const float width = 515;
        const float height = 180;
        const float marginLeft = 40;
        const float marginBottom = 30;
        const float marginTop = 15;
        const float marginRight = 10;
        var chartWidth = width - marginLeft - marginRight;
        var chartHeight = height - marginBottom - marginTop;

        int minVal, maxVal;
        if (chartType == "pulse")
        {
            minVal = Math.Max(0, readings.Min(r => r.Pulse) - 10);
            maxVal = readings.Max(r => r.Pulse) + 10;
        }
        else
        {
            minVal = Math.Max(0, readings.Min(r => Math.Min(r.Systolic, r.Diastolic)) - 10);
            maxVal = readings.Max(r => Math.Max(r.Systolic, r.Diastolic)) + 10;
        }

        var valueRange = maxVal - minVal;
        if (valueRange == 0) valueRange = 1;

        float ToY(int value) => marginTop + chartHeight - (chartHeight * (value - minVal) / valueRange);
        float ToX(int index) => readings.Count <= 1 ? marginLeft : marginLeft + (chartWidth * index / (readings.Count - 1));

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {width} {height}\">");

        // Grid lines and Y-axis labels
        var gridSteps = 5;
        for (var i = 0; i <= gridSteps; i++)
        {
            var y = marginTop + chartHeight - (chartHeight * i / gridSteps);
            var val = minVal + (valueRange * i / gridSteps);
            sb.AppendLine(CultureInfo.InvariantCulture, $"  <line x1=\"{marginLeft}\" y1=\"{y}\" x2=\"{marginLeft + chartWidth}\" y2=\"{y}\" stroke=\"#d0d0d0\" stroke-width=\"0.5\"/>");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  <text x=\"2\" y=\"{y + 3}\" font-size=\"8\" fill=\"#888\">{val}</text>");
        }

        // X-axis labels
        var labelInterval = Math.Max(1, readings.Count / 8);
        for (var i = 0; i < readings.Count; i += labelInterval)
        {
            var x = ToX(i);
            var label = readings[i].Timestamp.ToLocalTime().ToString("dd.MM.");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  <text x=\"{x - 12}\" y=\"{height - 5}\" font-size=\"8\" fill=\"#888\">{label}</text>");
        }

        if (chartType == "pulse")
        {
            sb.AppendLine($"  <polyline fill=\"none\" stroke=\"#2a9d8f\" stroke-width=\"2\" points=\"{BuildPoints(readings, r => r.Pulse, ToX, ToY)}\"/>");
        }
        else
        {
            sb.AppendLine($"  <polyline fill=\"none\" stroke=\"#e63946\" stroke-width=\"2\" points=\"{BuildPoints(readings, r => r.Systolic, ToX, ToY)}\"/>");
            sb.AppendLine($"  <polyline fill=\"none\" stroke=\"#457b9d\" stroke-width=\"2\" points=\"{BuildPoints(readings, r => r.Diastolic, ToX, ToY)}\"/>");

            // Legend
            sb.AppendLine(CultureInfo.InvariantCulture, $"  <line x1=\"{marginLeft + 5}\" y1=\"{marginTop + 2}\" x2=\"{marginLeft + 20}\" y2=\"{marginTop + 2}\" stroke=\"#e63946\" stroke-width=\"2\"/>");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  <text x=\"{marginLeft + 23}\" y=\"{marginTop + 6}\" font-size=\"9\" fill=\"#e63946\">Systole</text>");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  <line x1=\"{marginLeft + 75}\" y1=\"{marginTop + 2}\" x2=\"{marginLeft + 90}\" y2=\"{marginTop + 2}\" stroke=\"#457b9d\" stroke-width=\"2\"/>");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  <text x=\"{marginLeft + 93}\" y=\"{marginTop + 6}\" font-size=\"9\" fill=\"#457b9d\">Diastole</text>");
        }

        sb.AppendLine("</svg>");
        return sb.ToString();
    }

    private static string BuildPoints(List<BloodPressureReadingDto> readings, Func<BloodPressureReadingDto, int> selector, Func<int, float> toX, Func<int, float> toY)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < readings.Count; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(CultureInfo.InvariantCulture, $"{toX(i):F1},{toY(selector(readings[i])):F1}");
        }
        return sb.ToString();
    }

    private static void ComposeReadingsTable(IContainer container, List<BloodPressureReadingDto> readings)
    {
        container.Column(col =>
        {
            col.Item().Text("Alle Messungen").FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
            col.Item().PaddingTop(8).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2); // Date/Time
                    columns.RelativeColumn(1); // Systolic
                    columns.RelativeColumn(1); // Diastolic
                    columns.RelativeColumn(1); // Pulse
                    columns.RelativeColumn(2); // Notes
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Datum / Uhrzeit").FontSize(9).FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Systole").FontSize(9).FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Diastole").FontSize(9).FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Puls").FontSize(9).FontColor(Colors.White).Bold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Notizen").FontSize(9).FontColor(Colors.White).Bold();
                });

                var isAlternate = false;
                foreach (var reading in readings)
                {
                    var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                    table.Cell().Background(bgColor).Padding(4).Text(reading.Timestamp.ToLocalTime().ToString("dd.MM.yyyy HH:mm")).FontSize(9);
                    table.Cell().Background(bgColor).Padding(4).Text($"{reading.Systolic} mmHg").FontSize(9);
                    table.Cell().Background(bgColor).Padding(4).Text($"{reading.Diastolic} mmHg").FontSize(9);
                    table.Cell().Background(bgColor).Padding(4).Text($"{reading.Pulse} bpm").FontSize(9);
                    table.Cell().Background(bgColor).Padding(4).Text(reading.Notes ?? "").FontSize(9);

                    isAlternate = !isAlternate;
                }
            });
        });
    }
}
