using Verse;

using Mastery.Core.Utility;

namespace Mastery.Core.Data.Level_Framework.Extensions
{
    public class Level_Action_Extension : DefModExtension, IExposable
    {
        public string LevelKey = "Level"; //This is a Identifier for this Action.

        public UtilityCurve expGainCurve; //How much Exp is Gained.
        public OperationType expGainType;

        public void ExposeData()
        {
            Scribe_Values.Look(ref LevelKey, "levelKey");

            Scribe_Deep.Look(ref expGainCurve, "expGainCurve");
            Scribe_Values.Look(ref expGainType, "expGainType");
        }

        public void CopyTo(Level_Action_Extension target)
        {
            target.LevelKey = LevelKey;

            target.expGainCurve = expGainCurve.Duplicate();
            target.expGainType = expGainType;
        }

        public Level_Action_Extension Duplicate()
        {
            var duplicate = new Level_Action_Extension();

            CopyTo(duplicate);

            return duplicate;
        }
    }
}