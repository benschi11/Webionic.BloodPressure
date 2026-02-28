using System.ComponentModel.DataAnnotations;
using Webionic.BloodPressure.Data;

namespace Webionic.BloodPressure.Features.BloodPressure.Models;

public class BloodPressureReading
{
    public int Id { get; set; }

    [Required]
    [Range(60, 250, ErrorMessage = "Systolic must be between 60 and 250 mmHg.")]
    public int Systolic { get; set; }

    [Required]
    [Range(30, 150, ErrorMessage = "Diastolic must be between 30 and 150 mmHg.")]
    public int Diastolic { get; set; }

    [Required]
    [Range(30, 220, ErrorMessage = "Pulse must be between 30 and 220 bpm.")]
    public int Pulse { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Required]
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}
