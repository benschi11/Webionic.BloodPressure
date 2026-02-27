using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webionic.BloodPressure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodPressureEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Systole = table.Column<int>(type: "INTEGER", nullable: false),
                    Diastole = table.Column<int>(type: "INTEGER", nullable: false),
                    Pulse = table.Column<int>(type: "INTEGER", nullable: false),
                    MeasuredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodPressureEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReminderSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReminderTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    MondayEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    TuesdayEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    WednesdayEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ThursdayEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    FridayEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    SaturdayEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    SundayEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ReminderSettings",
                columns: new[] { "Id", "FridayEnabled", "IsEnabled", "MondayEnabled", "ReminderTime", "SaturdayEnabled", "SundayEnabled", "ThursdayEnabled", "TuesdayEnabled", "WednesdayEnabled" },
                values: new object[] { 1, true, false, true, new TimeOnly(8, 0, 0), true, true, true, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_BloodPressureEntries_MeasuredAt",
                table: "BloodPressureEntries",
                column: "MeasuredAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodPressureEntries");

            migrationBuilder.DropTable(
                name: "ReminderSettings");
        }
    }
}
