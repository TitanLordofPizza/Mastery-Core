using Mastery.Core.Utility;

namespace Mastery.Core.Data.Level_Framework
{
    public interface ILevel_Base
    {
        UtilityCurve TitleCurve { get; set; } //This is which Title will be Shown Per Level.
        UtilityCurve ExpCurve { get; set; } //This is how much Exp is Required Per Level.

        bool IsIgnored { get; set; } //This is to determine if the Item is Ignored.

        string MasteryCalculated(int Level, float Exp);

        float ExpCalculated(int Level);
    }
}