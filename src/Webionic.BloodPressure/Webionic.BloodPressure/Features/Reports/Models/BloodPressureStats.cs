namespace Webionic.BloodPressure.Features.Reports.Models;

public class BloodPressureStats
{
    public double AverageSystolic { get; set; }
    public double AverageDiastolic { get; set; }
    public double AveragePulse { get; set; }
    public int MaxSystolic { get; set; }
    public int MinSystolic { get; set; }
    public int MaxDiastolic { get; set; }
    public int MinDiastolic { get; set; }
    public int MaxPulse { get; set; }
    public int MinPulse { get; set; }
    public int TotalReadings { get; set; }
}
