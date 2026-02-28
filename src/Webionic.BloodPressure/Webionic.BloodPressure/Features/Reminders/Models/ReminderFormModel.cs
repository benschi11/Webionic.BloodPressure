using System.ComponentModel.DataAnnotations;

namespace Webionic.BloodPressure.Features.Reminders.Models;

public class ReminderFormModel
{
    [Required]
    public TimeOnly Time { get; set; } = new(8, 0);

    public bool Monday { get; set; } = true;
    public bool Tuesday { get; set; } = true;
    public bool Wednesday { get; set; } = true;
    public bool Thursday { get; set; } = true;
    public bool Friday { get; set; } = true;
    public bool Saturday { get; set; } = true;
    public bool Sunday { get; set; } = true;

    [MaxLength(200)]
    public string Label { get; set; } = "Blutdruck messen";
}
