using Mastery.Core.Utility;

namespace Mastery.Core.Data.Level_Framework
{
    public interface ILevel_Base
    {
        UtilityCurve TitleCurve { get; set; } //This is which Title will be Shown Per Level.
        UtilityCurve ExpCurve { get; set; } //This is how much Exp is Required Per Level.

        bool IsEnabledByDefault { get; set; } //This is to determine if the Item is Enabled by Default.

        string MasteryTranslation { get; set; } //This is whatever Translation Mastery Core will use when Mastery is Calculated.

        string MasteryCalculated(int Level, float Exp);

        float ExpCalculated(int Level);

        float CalculateField(string Field, int Level, float Base);
    }
}