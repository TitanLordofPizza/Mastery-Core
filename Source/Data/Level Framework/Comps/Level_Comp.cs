using System.Collections.Generic;

using UnityEngine;
using RimWorld;
using Verse;

using Mastery.Core.Settings.Level_Framework;
using Mastery.Core.Data.Level_Framework.Data;
using Mastery.Core.Data.Level_Framework.Data.Extensions;
using Mastery.Core.Data.Level_Framework.Extensions;

namespace Mastery.Core.Data.Level_Framework.Comps
{
    public class Level_Comp : ThingComp
    {
        #region Data

        public virtual string LevelKey => "Level"; //Key for what LevelType it is.

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
                    Level = 0,

                    Exp = 0,

                    Extensions = new Dictionary<string, Level_Data_Extension>()
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

                    entry.Level = (int)(config.ExpCurve.MaxX * Rand.ByCurve(generationLevelCurve));

                    if (majorPassions > 0)
                    {
                        entry.Passion = Passion.Major;

                        majorPassions--;
                    }
                    else if (minorPassions > 0)
                    {
                        entry.Passion = Passion.Minor;

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

            entry.Exp += (action.ExpGainCurve.Evaluate(entry.Level) * multiplier) * (1 + (entry.Passion == 0 ? 0 : 0.1f * (int)entry.Passion)); //Add Experience.

            while (true)
            {
                var evaluation = config.ExpCalculated(entry.Level); //Get Required Exp.

                if (entry.Level < config.ExpCurve.MaxX) //Can it use the Exp?
                {
                    if (entry.Exp >= evaluation) // Does it have Enough Exp?
                    {
                        entry.Level++; //Level Up.

                        entry.Exp = Mathf.Min(entry.Exp - evaluation); //Take away Used Exp.

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

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            parent.GetComp<Level_Comp_Manager>().Comps.Add(LevelKey, this);
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Collections.Look(ref Entries, "entries", LookMode.Value, LookMode.Deep);

            if (Entries == null)
                Entries = new Dictionary<string, Level_Data>();
        }

        #endregion

        #region UI

        public string GetDescription(string defName, bool Mastery = false)
        {
            var entry = GetOrAdd(defName);

            var settings = Level_Settings_Manager.Instances[LevelKey];

            var description = settings.GetLabelCap(defName) + (Mastery == false ? "" : $" - {settings.IGetConfig(defName).MasteryCalculated(entry.Level, entry.Exp)}");

            description += "\n\n";

            foreach (var key in entry.Extensions.Keys)
            {
                description += entry.Extensions[key].Description();
            }

            return description;
        }

        #endregion
    }
}