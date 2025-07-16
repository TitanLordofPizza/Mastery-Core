using System.Collections.Generic;

using RimWorld;
using Verse;

using Mastery.Core.Data.Level_Framework.Data.Extensions;

namespace Mastery.Core.Data.Level_Framework.Data
{
    public class Level_Data : IExposable
    {
        public int level;

        public float exp;

        public Passion passion;

        public Dictionary<string, Level_Data_Extension> extensions;

        public void ExposeData()
        {
            Scribe_Values.Look(ref level, "level");

            Scribe_Values.Look(ref exp, "exp");

            Scribe_Values.Look(ref passion, "passion");

            Scribe_Collections.Look(ref extensions, "extensions", LookMode.Value, LookMode.Deep);
        }
    }
}