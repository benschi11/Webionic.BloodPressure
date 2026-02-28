namespace Webionic.BloodPressure.Features.BloodPressure.Mappings;

using Webionic.BloodPressure.Features.BloodPressure.Models;

public static class BloodPressureMappingExtensions
{
    public static BloodPressureReadingDto ToDto(this BloodPressureReading entity)
    {
        return new BloodPressureReadingDto
        {
            Id = entity.Id,
            Systolic = entity.Systolic,
            Diastolic = entity.Diastolic,
            Pulse = entity.Pulse,
            Timestamp = entity.Timestamp,
            Notes = entity.Notes,
            UserId = entity.UserId
        };
    }

    public static List<BloodPressureReadingDto> ToDtoList(this IEnumerable<BloodPressureReading> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }

    public static BloodPressureReading ToEntity(this ReadingFormModel form, string userId)
    {
        return new BloodPressureReading
        {
            Systolic = form.Systolic,
            Diastolic = form.Diastolic,
            Pulse = form.Pulse,
            Notes = form.Notes,
            UserId = userId,
            Timestamp = DateTime.UtcNow
        };
    }
}
