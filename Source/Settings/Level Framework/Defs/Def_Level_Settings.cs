using System.Collections.Generic;

using Verse;

using Mastery.Core.Data.Level_Framework;
using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Data.Level_Framework.Defs;

using Mastery.Core.Settings.Level_Framework.Base;

namespace Mastery.Core.Settings.Level_Framework.Defs
{
    public class Def_Level_Settings<TComp, TDef> : Level_Settings where TComp : Level_Comp where TDef : LevelDef, IDuplicable<TDef>
    {
        public Dictionary<string, Level_Config<TDef>> Configs = new Dictionary<string, Level_Config<TDef>>();
        public Dictionary<string, TDef> tempDefs = new Dictionary<string, TDef>();

        #region Config

        public virtual void AddConfig(TDef def)
        {
            if (HasConfig(def.defName) == false)
            {
                Configs?.Add(def.defName, new Level_Config<TDef>()
                {
                    Value = null,//It only really needs its value set if its been Overrriden
                                 //so we will only add the data when that happens(to save up space).
                    Override = false
                });
            }

            tempDefs?.Add(def.defName, def);
        }

        public bool HasConfig(string defName)
        {
            return Configs?.ContainsKey(defName) == true;
        }

        public TDef GetConfig(string defName)
        {
            TDef def = null;

            if (defName.NullOrEmpty() == false)
            {
                Level_Config<TDef> config = null;
                Configs?.TryGetValue(defName, out config);

                if (config?.Value != null && config.Override == true)
                {
                    def = config.Value;
                }
                else
                {
                    tempDefs?.TryGetValue(defName, out def);
                }
            }

            return def;
        }

        public override ILevel_Base IGetConfig(string defName)
        {
            return GetConfig(defName);
        }

        public override TaggedString GetLabelCap(string defName)
        {
            var config = GetConfig(defName);

            if (config != null)
            {
                return config.LabelCap == null ? base.GetLabelCap(defName) : config.LabelCap;
            }

            return base.GetLabelCap(defName);
        }

        #endregion

        #region Active

        public bool ActiveOnThing(ThingWithComps thing)
        {
            return Active && thing != null && thing.HasComp<Level_Comp_Manager>() && thing.GetComp<Level_Comp_Manager>().Comps.ContainsKey(LevelKey);
        }

        public bool ActiveOnThing(ThingWithComps thing, out TComp comp)
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

        public bool ActiveOnThing(ThingWithComps thing, string defName, out TComp comp)
        {
            var activeOnThing = ActiveOnThing(thing, out comp);

            if (ActiveConfig(defName) == true)
                return activeOnThing;
            return false;
        }

        public bool ActiveConfig(string defName)
        {
            return HasConfig(defName) ? !GetConfig(defName).IsIgnored : false;
        }

        #endregion

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref Configs, "configs", LookMode.Value, LookMode.Deep);

            if (Configs == null)
                Configs = new Dictionary<string, Level_Config<TDef>>();
        }
    }
}