using System.Collections.Generic;

using Mastery.Core.Settings.Level_Framework.Base;

namespace Mastery.Core.Settings.Level_Framework
{
    public static class Level_Settings_Manager
    {
        public static Dictionary<string, Level_Settings> Instances = new Dictionary<string, Level_Settings>();

        public static void AddSettings(string levelKey, Level_Settings settings)
        {
            Instances.Add(levelKey, settings);
        }
    }
}