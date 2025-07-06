using Verse;

using Mastery.Core.Data.Level_Framework;

namespace Mastery.Core.Settings.Level_Framework
{
    public class Level_Config<TData> : IExposable where TData : ILevel_Base
    {
        public TData Value;
        public bool Override = false;

        public void ExposeData()
        {
            Scribe_Deep.Look(ref Value, "value");

            Scribe_Values.Look(ref Override, "override");
        }
    }
}