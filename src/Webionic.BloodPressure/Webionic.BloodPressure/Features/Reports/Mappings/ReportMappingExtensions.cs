namespace Webionic.BloodPressure.Features.Reports.Mappings;

using Webionic.BloodPressure.Features.BloodPressure.Models;
using Webionic.BloodPressure.Features.Reports.Models;

public static class ReportMappingExtensions
{
    public static BloodPressureStats ToStats(this IReadOnlyList<BloodPressureReading> readings)
    {
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
}
