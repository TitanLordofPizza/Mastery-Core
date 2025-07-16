using Verse;

using Mastery.Core.Utility;

namespace Mastery.Core.Data.Level_Framework.Defs
{
    public class LevelDef : Def, ILevel_Base, IDuplicable<LevelDef>
    {
        #region Variables

        #region Curves

        public UtilityCurve titleCurve;
        public UtilityCurve TitleCurve { get => titleCurve; set => titleCurve = value; }

        public UtilityCurve expCurve;
        public UtilityCurve ExpCurve { get => expCurve; set => expCurve = value; }

        #endregion

        #region Toggles

        public bool isIgnored;
        public bool IsIgnored { get => isIgnored; set => isIgnored = value; }

        #endregion

        #region Translations

        public string masteryTranslation = "Mastery_Core";
        public string MasteryTranslation { get => masteryTranslation; set => masteryTranslation = value; }

        #endregion

        #endregion

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

        public void CopyTo(LevelDef target)
        {
            target.titleCurve = titleCurve.Duplicate();

            target.expCurve = expCurve.Duplicate();

            target.isIgnored = isIgnored;

            target.masteryTranslation = masteryTranslation;
        }

        public LevelDef Duplicate()
        {
            var duplicate = new LevelDef();

            CopyTo(duplicate);

            return duplicate;
        }
    }
}