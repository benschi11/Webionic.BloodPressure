using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webionic.BloodPressure.Migrations;

/// <inheritdoc />
public partial class AddArmSideToBloodPressureReadings : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ArmSide",
            table: "BloodPressureReadings",
            type: "INTEGER",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ArmSide",
            table: "BloodPressureReadings");
    }
}
