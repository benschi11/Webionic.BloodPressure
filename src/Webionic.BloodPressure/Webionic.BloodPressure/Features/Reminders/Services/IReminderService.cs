using Webionic.BloodPressure.Features.Reminders.Models;

namespace Webionic.BloodPressure.Features.Reminders.Services;

public interface IReminderService
{
    Task<List<ReminderDto>> GetRemindersAsync(string userId);
    Task<ReminderDto?> GetReminderByIdAsync(int id, string userId);
    Task<ReminderDto> AddReminderAsync(ReminderFormModel form, string userId);
    Task UpdateReminderAsync(ReminderDto dto);
    Task DeleteReminderAsync(int id, string userId);
}
