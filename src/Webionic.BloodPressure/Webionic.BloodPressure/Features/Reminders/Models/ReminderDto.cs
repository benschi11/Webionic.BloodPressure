namespace Webionic.BloodPressure.Features.Reminders.Models;

public class ReminderDto
{
    public int Id { get; set; }
    public TimeOnly Time { get; set; }
    public bool Monday { get; set; }
    public bool Tuesday { get; set; }
    public bool Wednesday { get; set; }
    public bool Thursday { get; set; }
    public bool Friday { get; set; }
    public bool Saturday { get; set; }
    public bool Sunday { get; set; }
    public bool IsActive { get; set; }
    public string Label { get; set; } = default!;
    public string UserId { get; set; } = default!;
}
