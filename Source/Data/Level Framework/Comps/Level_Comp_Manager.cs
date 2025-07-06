using System.Collections.Generic;

using Verse;

using Mastery.Core.Data.Level_Framework.Extensions;

namespace Mastery.Core.Data.Level_Framework.Comps
{
    public class Level_Comp_Manager : ThingComp
    {
        public Dictionary<string, Level_Comp> Comps = new Dictionary<string, Level_Comp>();

        public List<string> ActionEvent(string actionType, Def def, Dictionary<string, object> states = null) //Action Keys are only really there to detect if any LevelKey was skipped over using Patches.
        {
            var actionKeys = new List<string>();

            if (def.modExtensions != null)
            {
                foreach (var _extension in def.modExtensions)
                {
                    if (_extension != null && _extension is Level_Action_Extension)
                    {
                        var extension = _extension as Level_Action_Extension;

                        actionKeys.Add(extension.LevelKey);

                        if (Comps.ContainsKey(extension.LevelKey))
                        {
                            Comps[extension.LevelKey].ActionEvent(def, extension, states);
                        }
                    }
                }
            }

            return actionKeys;
        }
    }
}