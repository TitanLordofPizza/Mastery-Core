using Verse;
//using VFECore;

namespace Mastery.Core.Data.Level_Framework.Data.Extensions
{
    public class Mastery_Mod_Extension : DefModExtension
    {
        public bool IgnoreDef = false;

        public static bool IsIgnored(Def def)
        {
            var extension = def.GetModExtension<Mastery_Mod_Extension>();

            if (extension != null)
            {
                return extension.IgnoreDef;
            }

            return false;
        }
    }
}