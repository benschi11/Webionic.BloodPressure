using Webionic.BloodPressure.Features.BloodPressure.Models;
using Webionic.BloodPressure.Features.Reports.Models;

namespace Webionic.BloodPressure.Features.Reports.Services;

public interface IPdfExportService
{
    byte[] GenerateReport(BloodPressureStats stats, List<BloodPressureReadingDto> readings, List<TimelineMarkerDto> markers, int days, int utcOffsetMinutes = 0);
}
