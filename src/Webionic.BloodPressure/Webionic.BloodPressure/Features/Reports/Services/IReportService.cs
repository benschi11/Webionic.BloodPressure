using Webionic.BloodPressure.Features.BloodPressure.Models;
using Webionic.BloodPressure.Features.Reports.Models;

namespace Webionic.BloodPressure.Features.Reports.Services;

public interface IReportService
{
    Task<BloodPressureStats> GetStatsAsync(string userId, DateTime? from = null, DateTime? to = null);
    Task<List<BloodPressureReadingDto>> GetReadingsForChartAsync(string userId, int days = 30);
}
