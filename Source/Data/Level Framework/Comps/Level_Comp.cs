using Mastery.Core.Data.Level_Framework.Data;
using Mastery.Core.Data.Level_Framework.Data.Extensions;
using Mastery.Core.Data.Level_Framework.Extensions;
using Mastery.Core.Settings.Level_Framework;
using Mastery.Core.UI.Tabs;
using Mastery.Core.Utility;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mastery.Core.Data.Level_Framework.Comps
{
    public class Level_Comp : ThingComp
    {
        #region Data

        public virtual string LevelKey => "Level"; //Key for what LevelType it is.

        public virtual IMastery_Tab Tab => null; //The Tab It Will Have In Mastery Tab If Any At All.

        public virtual SimpleCurve generationLevelCurve => new SimpleCurve
                    {
                        new CurvePoint(0f, 0f),
                        new CurvePoint(0.025f, 120f),
                        new CurvePoint(0.2f, 90f),
                        new CurvePoint(0.25f, 50f),
                        new CurvePoint(0.5f, 15f),
                        new CurvePoint(0.75f, 0f)
                    };

        public Dictionary<string, Level_Data> Entries; //Each Entry related to This Comp on the Subject.

        public Level_Comp()
        {
            Entries = new Dictionary<string, Level_Data>();
        }

        public virtual Level_Data GetOrAdd(string defName)
        {
            if (Entries.ContainsKey(defName) == false)
            {
                Entries.Add(defName, new Level_Data()
                {
                    level = 0,

                    exp = 0,

                    passion = 0,

                    extensions = new Dictionary<string, Level_Data_Extension>()
                });
            }

            return Entries[defName];
        }

        public virtual void GenerateEntries() //This is only meant for Generation Related Functions. 
        {
            if (Entries != null)
            {
                int majorPassions = 0;
                int minorPassions = 0;

                float rand = 5f + Mathf.Clamp(Rand.Gaussian(), -4f, 4f);

                while (rand >= 1f)
                {
                    if (rand >= 1.5f && Rand.Bool)
                    {
                        majorPassions++;
                        rand -= 1.5f;
                    }
                    else
                    {
                        minorPassions++;
                        rand -= 1f;
                    }
                }

                foreach (var entryKey in Entries.Keys)
                {
                    var entry = Entries[entryKey];

                    var config = Level_Settings_Manager.Instances[LevelKey].IGetConfig(entryKey);

                    entry.level = (int)(config.ExpCurve.MaxX * Rand.ByCurve(generationLevelCurve));

                    if (majorPassions > 0)
                    {
                        entry.passion = Passion.Major;

                        majorPassions--;
                    }
                    else if (minorPassions > 0)
                    {
                        entry.passion = Passion.Minor;

                        minorPassions--;
                    }
                }
            }
        }

        public virtual void ActionEvent(Def def, Level_Action_Extension action, Dictionary<string, object> states = null)
        {
            GainExperience(def, action);
        }

        public virtual bool GainExperience(Def def, Level_Action_Extension action, float multiplier = 1) //Adding Experience.
        {
            bool leveledUp = false;

            var entry = GetOrAdd(def.defName); //Get Level Data.

            var config = Level_Settings_Manager.Instances[LevelKey].IGetConfig(def.defName); //Get Config.

            entry.exp = MathUtility.OperateFloat(entry.exp, action.expGainCurve.Evaluate(entry.level) * multiplier * (1 + (0.1f * (int)entry.passion)), action.expGainType); //Add Experience.

            while (true)
            {
                var evaluation = config.ExpCalculated(entry.level); //Get Required Exp.

                if (entry.level < config.ExpCurve.MaxX) //Can it use the Exp?
                {
                    if (entry.exp >= evaluation) // Does it have Enough Exp?
                    {
                        entry.level++; //Level Up.

                        entry.exp = Mathf.Min(entry.exp - evaluation); //Take away Used Exp.

                        leveledUp = true;
                    }
                    else
                    {
                        break; //Stop Loop Can't Level Up.
                    }
                }
                else
                {
                    break; //Stop Loop Can't Level Up.
                }
            }

            return leveledUp;
        }

        public float CalculateField(string Entry, string Field, float Base)
        {
            return Level_Settings_Manager.Instances[LevelKey].IGetConfig(Entry).CalculateField(Field, GetOrAdd(Entry).level, Base);
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            parent.GetComp<Level_Comp_Manager>().Comps.Add(LevelKey, this);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Collections.Look(ref Entries, $"{LevelKey}entries", LookMode.Value, LookMode.Deep);

            if (Entries == null)
                Entries = new Dictionary<string, Level_Data>();
        }

        #endregion

        #region UI

        public bool HasTab()
        {
            return Tab != null && Level_Settings_Manager.Instances[LevelKey].Active == true && Level_Settings_Manager.Instances[LevelKey].TabActive == true;
        }

        public string GetDescription(string defName, bool Mastery = false)
        {
            var entry = GetOrAdd(defName);

            var settings = Level_Settings_Manager.Instances[LevelKey];

            var description = settings.GetLabelCap(defName) + (Mastery == false ? "" : $" - {settings.IGetConfig(defName).MasteryCalculated(entry.level, entry.exp)}");

            description += "\n\n";

            foreach (var key in entry.extensions.Keys)
            {
                description += entry.extensions[key].Description();
            }

            return description;
        }

        #endregion
    }
}