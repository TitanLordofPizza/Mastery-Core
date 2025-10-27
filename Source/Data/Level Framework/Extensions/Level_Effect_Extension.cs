using Mastery.Core.Data.Level_Framework.Defs;
using Mastery.Core.Settings.Level_Framework;
using Mastery.Core.Utility;
using Verse;

namespace Mastery.Core.Data.Level_Framework.Extensions
{
    public class Level_Effect_Extension : DefModExtension, IExposable, ILevel_Base, IDuplicable<Level_Effect_Extension>
    {
        #region Variables

        #region Curves

        public UtilityCurve titleCurve;
        public UtilityCurve TitleCurve { get => titleCurve; set => titleCurve = value; }

        public UtilityCurve expCurve;
        public UtilityCurve ExpCurve { get => expCurve; set => expCurve = value; }

        #endregion

        #region Toggles

        public bool isEnabledByDefault = true;
        public bool IsEnabledByDefault { get => isEnabledByDefault; set => isEnabledByDefault = value; }

        #endregion

        #region Translations

        public string masteryTranslation = "Mastery_Core";
        public string MasteryTranslation { get => masteryTranslation; set => masteryTranslation = value; }

        #endregion

        #endregion

        public virtual void ExposeData()
        {
            Scribe_Deep.Look(ref titleCurve, "titleCurve");

            Scribe_Deep.Look(ref expCurve, "expCurve");

            Scribe_Values.Look(ref isEnabledByDefault, "isIgnoredByDefault");

            Scribe_Values.Look(ref masteryTranslation, "masteryTranslation");
        }

        public string MasteryCalculated(int Level, float Exp)
        {
            return $"Level: {Level} ({Exp}/{ExpCalculated(Level)}) " + $"{masteryTranslation}_Level_Title{(int)TitleCurve.Evaluate(Level)}".Translate();
        }

        public float ExpCalculated(int Level)
        {
            return ExpCurve.Evaluate(Level);
        }

        public float CalculateField(string Field, int Level, float Base)
        {
            var curve = ClassUtility.GetField<UtilityCurve>(this, $"{Field}Curve");

            var evaluation = curve.Evaluate(Level);

            return MathUtility.OperateFloat(Base, curve.Percentage ? Base * evaluation : evaluation, ClassUtility.GetField<OperationType>(this, $"{Field}Type"));
        }

        public void CopyTo(Level_Effect_Extension target)
        {
            target.titleCurve = titleCurve.Duplicate();

            target.expCurve = expCurve.Duplicate();

            target.isEnabledByDefault = isEnabledByDefault;

            target.masteryTranslation = masteryTranslation;
        }

        public Level_Effect_Extension Duplicate()
        {
            var duplicate = new Level_Effect_Extension();

            CopyTo(duplicate);

            return duplicate;
        }
    }
}