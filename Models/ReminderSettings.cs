namespace Webionic.BloodPressure.Models;

public class ReminderSettings
{
    public int Id { get; set; }
    public bool IsEnabled { get; set; }
    public TimeOnly ReminderTime { get; set; } = new TimeOnly(8, 0);
    public bool MondayEnabled { get; set; } = true;
    public bool TuesdayEnabled { get; set; } = true;
    public bool WednesdayEnabled { get; set; } = true;
    public bool ThursdayEnabled { get; set; } = true;
    public bool FridayEnabled { get; set; } = true;
    public bool SaturdayEnabled { get; set; } = true;
    public bool SundayEnabled { get; set; } = true;
}
