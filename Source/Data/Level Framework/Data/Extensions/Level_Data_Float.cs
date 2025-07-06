using Verse;

using Mastery.Core.Utility;

namespace Mastery.Core.Data.Level_Framework.Data.Extensions
{
    public class Level_Data_Float : Level_Data_Extension
    {
        public float Value;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref Value, "value");
        }

        public override string Description()
        {
            return base.Description() + Math.RoundUp(Value, 2);
        }
    }
}