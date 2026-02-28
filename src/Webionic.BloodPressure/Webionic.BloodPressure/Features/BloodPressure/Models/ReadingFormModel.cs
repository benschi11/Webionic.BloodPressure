using System.ComponentModel.DataAnnotations;

namespace Webionic.BloodPressure.Features.BloodPressure.Models;

public class ReadingFormModel
{
    [Required]
    [Range(60, 250, ErrorMessage = "Systole muss zwischen 60 und 250 mmHg liegen.")]
    public int Systolic { get; set; }

    [Required]
    [Range(30, 150, ErrorMessage = "Diastole muss zwischen 30 und 150 mmHg liegen.")]
    public int Diastolic { get; set; }

    [Required]
    [Range(30, 220, ErrorMessage = "Puls muss zwischen 30 und 220 bpm liegen.")]
    public int Pulse { get; set; }

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.Now;

    [MaxLength(500)]
    public string? Notes { get; set; }
}
