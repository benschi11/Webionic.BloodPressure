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

    public async Task<List<TimelineMarkerDto>> GetTimelineMarkersAsync(string userId, int count = 100)
    {
        var markers = await context.TimelineMarkers
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.Timestamp)
            .Take(count)
            .ToListAsync();

        return markers.ToDtoList();
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

    public async Task<TimelineMarkerDto> AddTimelineMarkerAsync(TimelineMarkerFormModel form, string userId)
    {
        var entity = form.ToEntity(userId);
        context.TimelineMarkers.Add(entity);
        await context.SaveChangesAsync();
        return entity.ToDto();
    }

    public async Task<bool> UpdateReadingAsync(int id, ReadingFormModel form, string userId)
    {
        var reading = await context.BloodPressureReadings
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        if (reading is null) return false;

        reading.Systolic = form.Systolic;
        reading.Diastolic = form.Diastolic;
        reading.Pulse = form.Pulse;
        reading.Notes = form.Notes;
        reading.ArmSide = form.ArmSide;
        reading.Timestamp = form.Timestamp.ToUniversalTime();

        await context.SaveChangesAsync();
        return true;
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

    public async Task DeleteTimelineMarkerAsync(int id, string userId)
    {
        var marker = await context.TimelineMarkers
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

        if (marker is not null)
        {
            context.TimelineMarkers.Remove(marker);
            await context.SaveChangesAsync();
        }
    }
}
