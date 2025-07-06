using Verse;

using Mastery.Core.Utility;

namespace Mastery.Core.Data.Level_Framework.Extensions
{
    public class Level_Action_Extension : DefModExtension, IExposable
    {
        public string LevelKey = "Level"; //This is a Identifier for this Action.

        public UtilityCurve ExpGainCurve; //How much Exp is Gained.

        public void ExposeData()
        {
            Scribe_Values.Look(ref LevelKey, "levelKey");

            Scribe_Deep.Look(ref ExpGainCurve, "expGainCurve");
        }
    }
}