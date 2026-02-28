using Microsoft.EntityFrameworkCore;
using Webionic.BloodPressure.Data;
using Webionic.BloodPressure.Features.BloodPressure.Mappings;
using Webionic.BloodPressure.Features.BloodPressure.Models;

namespace Webionic.BloodPressure.Features.BloodPressure.Services;

public class BloodPressureService(ApplicationDbContext context) : IBloodPressureService
{
    public async Task<List<BloodPressureReadingDto>> GetReadingsAsync(string userId, int count = 50)
    {
        var readings = await context.BloodPressureReadings
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.Timestamp)
            .Take(count)
            .ToListAsync();

        return readings.ToDtoList();
    }

    public async Task<BloodPressureReadingDto?> GetReadingByIdAsync(int id, string userId)
    {
        var reading = await context.BloodPressureReadings
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        return reading?.ToDto();
    }

    public async Task<BloodPressureReadingDto> AddReadingAsync(ReadingFormModel form, string userId)
    {
        var entity = form.ToEntity(userId);
        context.BloodPressureReadings.Add(entity);
        await context.SaveChangesAsync();
        return entity.ToDto();
    }

    public async Task DeleteReadingAsync(int id, string userId)
    {
        var reading = await context.BloodPressureReadings
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        if (reading is not null)
        {
            context.BloodPressureReadings.Remove(reading);
            await context.SaveChangesAsync();
        }
    }
}
