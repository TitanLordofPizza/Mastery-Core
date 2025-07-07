using Mastery.Core.Utility;
using Verse;

namespace Mastery.Core.Data.Level_Framework.Defs
{
    public class LevelDef : Def, ILevel_Base, IDuplicable<LevelDef>
    {
        public UtilityCurve titleCurve;
        public UtilityCurve TitleCurve { get => titleCurve; set => titleCurve = value; }

        public UtilityCurve expCurve;
        public UtilityCurve ExpCurve { get => expCurve; set => expCurve = value; }

        public bool isIgnored;
        public bool IsIgnored { get => isIgnored; set => isIgnored = value; }

        public string MasteryCalculated(int Level, float Exp)
        {
            return $"Level: {Level}({Exp}/{ExpCalculated(Level)}) " + $"Mastery_Core_Level_Title{(int)TitleCurve.Evaluate(Level)}".Translate(); //Right after this would be the title
        }

        public float ExpCalculated(int Level)
        {
            return ExpCurve.Evaluate(Level);
        }

        public void CopyTo(LevelDef target)
        {
            target.titleCurve = titleCurve.Duplicate();

            target.expCurve = expCurve.Duplicate();

            target.isIgnored = isIgnored;
        }

        public LevelDef Duplicate()
        {
            var duplicate = new LevelDef();

            CopyTo(duplicate);

            return duplicate;
        }
    }
}