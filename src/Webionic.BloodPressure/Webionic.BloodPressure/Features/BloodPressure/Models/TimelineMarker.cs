using System.ComponentModel.DataAnnotations;
using Webionic.BloodPressure.Data;

namespace Webionic.BloodPressure.Features.BloodPressure.Models;

public class TimelineMarker
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = default!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}
