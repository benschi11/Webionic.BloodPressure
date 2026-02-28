using Microsoft.EntityFrameworkCore;
using Webionic.BloodPressure.Data;
using Webionic.BloodPressure.Features.BloodPressure.Models;
using Webionic.BloodPressure.Features.Reports.Models;

namespace Webionic.BloodPressure.Features.Reports.Services;

public class ReportService(ApplicationDbContext context) : IReportService
{
    public async Task<BloodPressureStats> GetStatsAsync(string userId, DateTime? from = null, DateTime? to = null)
    {
        var query = context.BloodPressureReadings
            .Where(r => r.UserId == userId);

        if (from.HasValue)
            query = query.Where(r => r.Timestamp >= from.Value);
        if (to.HasValue)
            query = query.Where(r => r.Timestamp <= to.Value);

        var readings = await query.ToListAsync();

        if (readings.Count == 0)
        {
            return new BloodPressureStats();
        }

        return new BloodPressureStats
        {
            AverageSystolic = Math.Round(readings.Average(r => r.Systolic), 1),
            AverageDiastolic = Math.Round(readings.Average(r => r.Diastolic), 1),
            AveragePulse = Math.Round(readings.Average(r => r.Pulse), 1),
            MaxSystolic = readings.Max(r => r.Systolic),
            MinSystolic = readings.Min(r => r.Systolic),
            MaxDiastolic = readings.Max(r => r.Diastolic),
            MinDiastolic = readings.Min(r => r.Diastolic),
            MaxPulse = readings.Max(r => r.Pulse),
            MinPulse = readings.Min(r => r.Pulse),
            TotalReadings = readings.Count
        };
    }

    public async Task<List<BloodPressureReading>> GetReadingsForChartAsync(string userId, int days = 30)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);

        return await context.BloodPressureReadings
            .Where(r => r.UserId == userId && r.Timestamp >= fromDate)
            .OrderBy(r => r.Timestamp)
            .ToListAsync();
    }
}
