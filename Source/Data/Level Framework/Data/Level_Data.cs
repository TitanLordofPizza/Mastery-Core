using System.Collections.Generic;

using RimWorld;
using Verse;

using Mastery.Core.Data.Level_Framework.Data.Extensions;

namespace Mastery.Core.Data.Level_Framework.Data
{
    public class Level_Data : IExposable
    {
        public int Level;

        public float Exp;

        public Passion Passion;

        public Dictionary<string, Level_Data_Extension> Extensions;

        public void ExposeData()
        {
            Scribe_Values.Look(ref Level, "level");

            Scribe_Values.Look(ref Exp, "exp");

            Scribe_Values.Look(ref Passion, "passion");

            Scribe_Collections.Look(ref Extensions, "extensions", LookMode.Value, LookMode.Deep);
        }
    }
}