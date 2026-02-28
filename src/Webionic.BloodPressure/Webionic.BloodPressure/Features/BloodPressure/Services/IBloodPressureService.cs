using Webionic.BloodPressure.Features.BloodPressure.Models;

namespace Webionic.BloodPressure.Features.BloodPressure.Services;

public interface IBloodPressureService
{
    Task<List<BloodPressureReading>> GetReadingsAsync(string userId, int count = 50);
    Task<BloodPressureReading?> GetReadingByIdAsync(int id, string userId);
    Task AddReadingAsync(BloodPressureReading reading);
    Task DeleteReadingAsync(int id, string userId);
}
