using Microsoft.EntityFrameworkCore;
using Webionic.BloodPressure.Data;
using Webionic.BloodPressure.Features.BloodPressure.Mappings;
using Webionic.BloodPressure.Features.BloodPressure.Models;
using Webionic.BloodPressure.Features.Reports.Mappings;
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

        return ((IReadOnlyList<BloodPressureReading>)readings).ToStats();
    }

    public async Task<List<BloodPressureReadingDto>> GetReadingsForChartAsync(string userId, int days = 30)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);

        var readings = await context.BloodPressureReadings
            .Where(r => r.UserId == userId && r.Timestamp >= fromDate)
            .OrderBy(r => r.Timestamp)
            .ToListAsync();

        return readings.ToDtoList();
    }
}
