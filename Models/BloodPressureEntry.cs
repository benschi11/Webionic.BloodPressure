using System.ComponentModel.DataAnnotations;

namespace Webionic.BloodPressure.Models;

public class BloodPressureEntry
{
    public int Id { get; set; }

    [Range(50, 300, ErrorMessage = "Systole muss zwischen 50 und 300 liegen.")]
    public int Systole { get; set; }

    [Range(30, 200, ErrorMessage = "Diastole muss zwischen 30 und 200 liegen.")]
    public int Diastole { get; set; }

    [Range(30, 250, ErrorMessage = "Puls muss zwischen 30 und 250 liegen.")]
    public int Pulse { get; set; }

    public DateTime MeasuredAt { get; set; } = DateTime.Now;

    public string? Note { get; set; }

    public string BloodPressureCategory =>
        Systole >= 180 || Diastole >= 120 ? "Hypertensiver Notfall" :
        Systole >= 140 || Diastole >= 90 ? "Hypertonie Grad 2" :
        Systole >= 130 || Diastole >= 80 ? "Hypertonie Grad 1" :
        Systole >= 120 && Diastole < 80 ? "Erhöht" :
        "Normal";

    public string CategoryCssClass =>
        Systole >= 180 || Diastole >= 120 ? "danger" :
        Systole >= 140 || Diastole >= 90 ? "warning" :
        Systole >= 130 || Diastole >= 80 ? "info" :
        Systole >= 120 && Diastole < 80 ? "primary" :
        "success";
}
