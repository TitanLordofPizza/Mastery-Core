using Verse;

using Mastery.Core.Data.Level_Framework;

namespace Mastery.Core.Settings.Level_Framework
{
    public class Level_Config<TData> : IExposable where TData : ILevel_Base, IDuplicable<TData>
    {
        public TData Value;
        public bool Override = false;

        public void ExposeData()
        {
            Scribe_Deep.Look(ref Value, "value");

            Scribe_Values.Look(ref Override, "override");
        }

        public Level_Config<TData> Duplicate()
        {
            return new Level_Config<TData>()
            {
                Value = Value.Duplicate(),

                Override = Override
            };
        }
    }
}