namespace Webionic.BloodPressure.Features.Reminders.Mappings;

using Webionic.BloodPressure.Features.Reminders.Models;

public static class ReminderMappingExtensions
{
    public static ReminderDto ToDto(this Reminder entity)
    {
        return new ReminderDto
        {
            Id = entity.Id,
            Time = entity.Time,
            Monday = entity.Monday,
            Tuesday = entity.Tuesday,
            Wednesday = entity.Wednesday,
            Thursday = entity.Thursday,
            Friday = entity.Friday,
            Saturday = entity.Saturday,
            Sunday = entity.Sunday,
            IsActive = entity.IsActive,
            Label = entity.Label,
            UserId = entity.UserId
        };
    }

    public static List<ReminderDto> ToDtoList(this IEnumerable<Reminder> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }

    public static Reminder ToEntity(this ReminderFormModel form, string userId)
    {
        return new Reminder
        {
            Time = form.Time,
            Monday = form.Monday,
            Tuesday = form.Tuesday,
            Wednesday = form.Wednesday,
            Thursday = form.Thursday,
            Friday = form.Friday,
            Saturday = form.Saturday,
            Sunday = form.Sunday,
            IsActive = true,
            Label = form.Label,
            UserId = userId
        };
    }

    public static void UpdateFromDto(this Reminder entity, ReminderDto dto)
    {
        entity.Time = dto.Time;
        entity.Monday = dto.Monday;
        entity.Tuesday = dto.Tuesday;
        entity.Wednesday = dto.Wednesday;
        entity.Thursday = dto.Thursday;
        entity.Friday = dto.Friday;
        entity.Saturday = dto.Saturday;
        entity.Sunday = dto.Sunday;
        entity.IsActive = dto.IsActive;
        entity.Label = dto.Label;
    }
}
