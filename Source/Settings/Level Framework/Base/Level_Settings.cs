using Verse;

using Mastery.Core.Data.Level_Framework;
using Mastery.Core.Data.Level_Framework.Extensions;

namespace Mastery.Core.Settings.Level_Framework.Base
{
    public class Level_Settings : IExposable
    {
        public virtual string LevelKey => "Level";

        public bool Active = true; //Is this Level Settings Enabled?

        public bool TabActive = false; // Is This Level Systems Tabs Enabled?

        public Level_Action_Extension ActionBase;

        public virtual ILevel_Base IGetConfig(string defName)
        {
            return null;
        }

        public virtual TaggedString GetLabelCap(string defName)
        {
            return defName.CapitalizeFirst();
        }

        public virtual void Initilize()
        {
            Level_Settings_Manager.AddSettings(LevelKey, this);
        }

        public virtual void ExposeData()
        {
            Scribe_Values.Look(ref Active, "active");
            Scribe_Values.Look(ref TabActive, "tabActive");

            Scribe_Deep.Look(ref ActionBase, "action");
        }
    }
}