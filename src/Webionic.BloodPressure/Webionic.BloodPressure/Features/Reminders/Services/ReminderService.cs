using Microsoft.EntityFrameworkCore;
using Webionic.BloodPressure.Data;
using Webionic.BloodPressure.Features.Reminders.Mappings;
using Webionic.BloodPressure.Features.Reminders.Models;

namespace Webionic.BloodPressure.Features.Reminders.Services;

public class ReminderService(ApplicationDbContext context) : IReminderService
{
    public async Task<List<ReminderDto>> GetRemindersAsync(string userId)
    {
        var reminders = await context.Reminders
            .Where(r => r.UserId == userId)
            .OrderBy(r => r.Time)
            .ToListAsync();

        return reminders.ToDtoList();
    }

    public async Task<ReminderDto?> GetReminderByIdAsync(int id, string userId)
    {
        var reminder = await context.Reminders
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        return reminder?.ToDto();
    }

    public async Task<ReminderDto> AddReminderAsync(ReminderFormModel form, string userId)
    {
        var entity = form.ToEntity(userId);
        context.Reminders.Add(entity);
        await context.SaveChangesAsync();
        return entity.ToDto();
    }

    public async Task UpdateReminderAsync(ReminderDto dto)
    {
        var entity = await context.Reminders
            .FirstOrDefaultAsync(r => r.Id == dto.Id && r.UserId == dto.UserId);

        if (entity is not null)
        {
            entity.UpdateFromDto(dto);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteReminderAsync(int id, string userId)
    {
        var reminder = await context.Reminders
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        if (reminder is not null)
        {
            context.Reminders.Remove(reminder);
            await context.SaveChangesAsync();
        }
    }
}
