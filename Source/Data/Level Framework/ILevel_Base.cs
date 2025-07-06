using Mastery.Core.Utility;

namespace Mastery.Core.Data.Level_Framework
{
    public interface ILevel_Base
    {
        UtilityCurve TitleCurve { get; set; } //This is which Title will be Shown Per Level.
        UtilityCurve ExpCurve { get; set; } //This is how much Exp is Required Per Level.

        string MasteryCalculated(int Level, float Exp);

        float ExpCalculated(int Level);
    }
}