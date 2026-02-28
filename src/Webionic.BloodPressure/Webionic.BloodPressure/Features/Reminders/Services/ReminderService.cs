using Microsoft.EntityFrameworkCore;
using Webionic.BloodPressure.Data;
using Webionic.BloodPressure.Features.Reminders.Models;

namespace Webionic.BloodPressure.Features.Reminders.Services;

public class ReminderService(ApplicationDbContext context) : IReminderService
{
    public async Task<List<Reminder>> GetRemindersAsync(string userId)
    {
        return await context.Reminders
            .Where(r => r.UserId == userId)
            .OrderBy(r => r.Time)
            .ToListAsync();
    }

    public async Task<Reminder?> GetReminderByIdAsync(int id, string userId)
    {
        return await context.Reminders
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
    }

    public async Task AddReminderAsync(Reminder reminder)
    {
        context.Reminders.Add(reminder);
        await context.SaveChangesAsync();
    }

    public async Task UpdateReminderAsync(Reminder reminder)
    {
        context.Reminders.Update(reminder);
        await context.SaveChangesAsync();
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
