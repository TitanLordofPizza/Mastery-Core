using System.Collections.Generic;

using Verse;

using Mastery.Core.Data.Level_Framework;
using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Data.Level_Framework.Extensions;

using Mastery.Core.Settings.Level_Framework.Base;

namespace Mastery.Core.Settings.Level_Framework.Extensions
{
    public class Extension_Level_Settings<TComp, TExtension> : Level_Settings where TComp : Level_Comp where TExtension : Level_Effect_Extension
    {
        public TExtension ExtensionBase;

        public Dictionary<string, Level_Config<TExtension>> Configs = new Dictionary<string, Level_Config<TExtension>>();
        public Dictionary<string, Def> tempDefs = new Dictionary<string, Def>();

        #region Config

        public virtual void AddConfig(Def def)
        {
            if (HasConfig(def.defName) == false)
            {
                Configs?.Add(def.defName, new Level_Config<TExtension>()
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

        public TExtension GetConfig(string defName)
        {
            TExtension extension = null;

            if (defName.NullOrEmpty() == false)
            {
                Level_Config<TExtension> configValue = null;
                Configs?.TryGetValue(defName, out configValue);

                if (configValue != null && configValue.Override == true)
                    extension = configValue.Value;
                else
                {
                    Def def = null;
                    tempDefs?.TryGetValue(defName, out def);

                    if (def != null)
                        extension = def.GetModExtension<TExtension>();
                }

                if (extension == null)
                    extension = ExtensionBase;
            }
            else
            {
                extension = ExtensionBase;
            }

            return extension;
        }

        public override ILevel_Base IGetConfig(string defName)
        {
            return GetConfig(defName);
        }

        public override TaggedString GetLabelCap(string defName)
        {
            Def def = null;
            tempDefs?.TryGetValue(defName, out def);
            
            if(def != null)
            {
                return def.LabelCap == null ? base.GetLabelCap(defName) : def.LabelCap;
            }

            return base.GetLabelCap(defName);
        }

        #endregion

        #region Active

        public bool ActiveOnThing(ThingWithComps thing)
        {
            return Active && thing.HasComp<Level_Comp_Manager>() && thing.GetComp<Level_Comp_Manager>().Comps.ContainsKey(LevelKey);
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

        #endregion

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Active, "active");

            Scribe_Deep.Look(ref ExtensionBase, "base");

            Scribe_Deep.Look(ref ActionBase, "action");

            Scribe_Collections.Look(ref Configs, "configs", LookMode.Value, LookMode.Deep);

            if (Configs == null)
                Configs = new Dictionary<string, Level_Config<TExtension>>();
        }
    }
}