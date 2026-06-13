namespace Webionic.BloodPressure.Features.BloodPressure.Models;

public static class ArmSideExtensions
{
    public static string ToDisplayText(this ArmSide? armSide, string unspecifiedText = "Nicht angegeben")
    {
        return armSide switch
        {
            ArmSide.Left => "Links",
            ArmSide.Right => "Rechts",
            _ => unspecifiedText
        };
    }
}
