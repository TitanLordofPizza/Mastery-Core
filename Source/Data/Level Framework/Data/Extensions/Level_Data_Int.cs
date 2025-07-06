using Verse;

namespace Mastery.Core.Data.Level_Framework.Data.Extensions
{
    public class Level_Data_Int : Level_Data_Extension
    {
        public int Value;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref Value, "value");
        }

        public override string Description()
        {
            return base.Description() + Value;
        }
    }
}