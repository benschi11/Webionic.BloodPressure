using Webionic.BloodPressure.Features.BloodPressure.Models;

namespace Webionic.BloodPressure.Features.BloodPressure.Services;

public interface IBloodPressureService
{
    Task<List<BloodPressureReadingDto>> GetReadingsAsync(string userId, int count = 50);
    Task<List<TimelineMarkerDto>> GetTimelineMarkersAsync(string userId, int count = 100, DateTime? fromDate = null);
    Task<BloodPressureReadingDto?> GetReadingByIdAsync(int id, string userId);
    Task<BloodPressureReadingDto> AddReadingAsync(ReadingFormModel form, string userId);
    Task<TimelineMarkerDto> AddTimelineMarkerAsync(TimelineMarkerFormModel form, string userId);
    Task<bool> UpdateReadingAsync(int id, ReadingFormModel form, string userId);
    Task DeleteReadingAsync(int id, string userId);
    Task DeleteTimelineMarkerAsync(int id, string userId);
}
