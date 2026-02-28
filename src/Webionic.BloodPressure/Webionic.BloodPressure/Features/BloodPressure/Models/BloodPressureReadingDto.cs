namespace Webionic.BloodPressure.Features.BloodPressure.Models;

public class BloodPressureReadingDto
{
    public int Id { get; set; }
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public int Pulse { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Notes { get; set; }
    public string UserId { get; set; } = default!;
}
