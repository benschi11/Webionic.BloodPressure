using System.ComponentModel.DataAnnotations;

namespace Webionic.BloodPressure.Features.BloodPressure.Models;

public class TimelineMarkerFormModel
{
    [Required(ErrorMessage = "Bitte eine Bezeichnung für die Marke eingeben.")]
    [MaxLength(200, ErrorMessage = "Die Bezeichnung darf maximal 200 Zeichen lang sein.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
