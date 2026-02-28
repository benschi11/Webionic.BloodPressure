using Webionic.BloodPressure.Features.BloodPressure.Models;

namespace Webionic.BloodPressure.Features.BloodPressure.Services;

public interface IBloodPressureService
{
    Task<List<BloodPressureReadingDto>> GetReadingsAsync(string userId, int count = 50);
    Task<BloodPressureReadingDto?> GetReadingByIdAsync(int id, string userId);
    Task<BloodPressureReadingDto> AddReadingAsync(ReadingFormModel form, string userId);
    Task DeleteReadingAsync(int id, string userId);
}
