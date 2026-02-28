using Webionic.BloodPressure.Features.Reminders.Models;

namespace Webionic.BloodPressure.Features.Reminders.Services;

public interface IReminderService
{
    Task<List<Reminder>> GetRemindersAsync(string userId);
    Task<Reminder?> GetReminderByIdAsync(int id, string userId);
    Task AddReminderAsync(Reminder reminder);
    Task UpdateReminderAsync(Reminder reminder);
    Task DeleteReminderAsync(int id, string userId);
}
