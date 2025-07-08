using Verse;

using Mastery.Core.Utility;

namespace Mastery.Core.Data.Level_Framework.Extensions
{
    public class Level_Effect_Extension : DefModExtension, IExposable, ILevel_Base, IDuplicable<Level_Effect_Extension>
    {
        public UtilityCurve titleCurve;
        public UtilityCurve TitleCurve { get => titleCurve; set => titleCurve = value; }

        public UtilityCurve expCurve;
        public UtilityCurve ExpCurve { get => expCurve; set => expCurve = value; }

        public bool isIgnored;
        public bool IsIgnored { get => isIgnored; set => isIgnored = value; }

        public virtual void ExposeData()
        {
            Scribe_Deep.Look(ref titleCurve, "titleCurve");

            Scribe_Deep.Look(ref expCurve, "expCurve");

            Scribe_Values.Look(ref isIgnored, "isIgnored");
        }

        public string MasteryCalculated(int Level, float Exp)
        {
            return $"Level: {Level} ({Exp}/{ExpCalculated(Level)}) " + $"Mastery_Core_Level_Title{(int)TitleCurve.Evaluate(Level)}".Translate(); //Right after this would be the title
        }

        public float ExpCalculated(int Level)
        {
            return ExpCurve.Evaluate(Level);
        }

        public void CopyTo(Level_Effect_Extension target)
        {
            target.titleCurve = titleCurve.Duplicate();

            target.expCurve = expCurve.Duplicate();

            target.isIgnored = isIgnored;
        }

        public Level_Effect_Extension Duplicate()
        {
            var duplicate = new Level_Effect_Extension();

            CopyTo(duplicate);

            return duplicate;
        }
    }
}