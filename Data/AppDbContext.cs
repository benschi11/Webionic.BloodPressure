using Microsoft.EntityFrameworkCore;
using Webionic.BloodPressure.Models;

namespace Webionic.BloodPressure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BloodPressureEntry> BloodPressureEntries { get; set; }
    public DbSet<ReminderSettings> ReminderSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BloodPressureEntry>()
            .HasIndex(e => e.MeasuredAt);

        modelBuilder.Entity<ReminderSettings>()
            .HasData(new ReminderSettings { Id = 1 });
    }
}
