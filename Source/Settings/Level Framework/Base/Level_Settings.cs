using Verse;

using Mastery.Core.Data.Level_Framework;
using Mastery.Core.Data.Level_Framework.Comps;
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

        public bool ActiveConfig(string defName)
        {
            return HasConfig(defName) ? !IGetConfig(defName).IsIgnored : false;
        }

        public virtual bool HasConfig(string defName)
        {
            return false;
        }

        public virtual TaggedString GetLabelCap(string defName)
        {
            return defName.CapitalizeFirst();
        }

        public virtual void Initilize()
        {
            Level_Settings_Manager.AddSettings(LevelKey, this);
        }

        public bool ActiveOnThing(ThingWithComps thing)
        {
            return Active && thing != null && thing.HasComp<Level_Comp_Manager>() && thing.GetComp<Level_Comp_Manager>().Comps.ContainsKey(LevelKey);
        }

        public bool ActiveOnThing(ThingWithComps thing, string defName)
        {
            var activeOnThing = ActiveOnThing(thing);

            if (ActiveConfig(defName) == true)
            {
                return activeOnThing;
            }

            return false;
        }

        protected bool ActiveOnThing<TComp>(ThingWithComps thing, out TComp comp) where TComp : Level_Comp
        {
            var state = ActiveOnThing(thing);

            if (state == true)
            {
                comp = thing.GetComp<Level_Comp_Manager>().Comps[LevelKey] as TComp;
            }
            else
            {
                comp = null;
            }

            return state;
        }

        protected bool ActiveOnThing<TComp>(ThingWithComps thing, string defName, out TComp comp) where TComp : Level_Comp
        {
            var state = ActiveOnThing(thing, defName);

            if (state == true)
            {
                comp = thing.GetComp<Level_Comp_Manager>().Comps[LevelKey] as TComp;
            }
            else
            {
                comp = null;
            }

            return state;
        }

        public virtual void ExposeData()
        {
            Scribe_Values.Look(ref Active, "active");
            Scribe_Values.Look(ref TabActive, "tabActive");

            Scribe_Deep.Look(ref ActionBase, "action");
        }
    }
}