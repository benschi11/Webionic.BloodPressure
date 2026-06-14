namespace Webionic.BloodPressure.Features.BloodPressure.Models;

public class TimelineMarkerDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; } = default!;
}
