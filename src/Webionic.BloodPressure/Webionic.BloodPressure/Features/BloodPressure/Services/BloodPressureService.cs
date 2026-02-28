using Microsoft.EntityFrameworkCore;
using Webionic.BloodPressure.Data;
using Webionic.BloodPressure.Features.BloodPressure.Models;

namespace Webionic.BloodPressure.Features.BloodPressure.Services;

public class BloodPressureService(ApplicationDbContext context) : IBloodPressureService
{
    public async Task<List<BloodPressureReading>> GetReadingsAsync(string userId, int count = 50)
    {
        return await context.BloodPressureReadings
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.Timestamp)
            .Take(count)
            .ToListAsync();
    }

    public async Task<BloodPressureReading?> GetReadingByIdAsync(int id, string userId)
    {
        return await context.BloodPressureReadings
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
    }

    public async Task AddReadingAsync(BloodPressureReading reading)
    {
        context.BloodPressureReadings.Add(reading);
        await context.SaveChangesAsync();
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
