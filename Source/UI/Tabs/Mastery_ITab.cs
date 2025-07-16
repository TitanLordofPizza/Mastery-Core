using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Settings.Level_Framework;
using Mastery.Core.Utility.UI;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mastery.Core.UI.Tabs
{
    [StaticConstructorOnStartup]
    public class Mastery_ITab : ITab
    {
        static Mastery_ITab()
        {
            var defs = DefDatabase<ThingDef>.AllDefsListForReading;

            foreach (var def in defs)
            {
                if (def.race != null && def.race.Humanlike == true)
                {
                    def.inspectorTabs?.Add(typeof(Mastery_ITab));
                    def.inspectorTabsResolved?.Add(InspectTabManager.GetSharedInstance(typeof(Mastery_ITab)));
                }
            }
        }

        public Mastery_ITab()
        {
            size = new Vector2(505, 515);
            labelKey = "Mastery_Tab";
        }

        public override bool IsVisible
        {
            get
            {
                if (Find.Selector.SingleSelectedThing is Pawn pawn)
                {
                    foreach (var comp in pawn.GetComp<Level_Comp_Manager>().Comps.Values) // Look For Active Mastery Systems.
                    {
                        if (comp.HasTab() == true) // Does This Have Any Active Mastery System?
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public int selectedTab;

        protected override void FillTab()
        {
            if (Find.Selector.SingleSelectedThing is Pawn pawn)
            {
                var rect = new Rect(0f, UIUtility.largeUISpacing, size.x, size.y - UIUtility.largeUISpacing);
                var inRect = new Rect(0f, rect.y + UIUtility.largeUISpacing, rect.width, rect.height - UIUtility.largeUISpacing);

                var tabs = new List<TabRecord>();

                var comp_manager = pawn.GetComp<Level_Comp_Manager>();
                var comps = comp_manager.Comps.Values.ToArray();

                bool containsSelected = false;
                int firstIndex = -1;

                for (int i = 0; i < comp_manager.Comps.Count; i++)  // Add Active Mastery Systems.
                {
                    var comp = comps[i];

                    if (comp.HasTab() == true) // Is This An Active Mastery System?
                    {
                        int tabIndex = i;

                        tabs.Add(new TabRecord(comp.Tab.Title.Translate(), () =>
                        {
                            selectedTab = tabIndex;
                        }, selectedTab == tabIndex));

                        if (firstIndex == -1)
                        {
                            firstIndex = tabIndex;
                        }

                        if (selectedTab == tabIndex)
                        {
                            containsSelected = true;
                        }
                    }
                }

                if (containsSelected == false)
                {
                    selectedTab = firstIndex;
                    containsSelected = true;
                }

                UIUtility.Tabs(rect, tabs);

                if (containsSelected == true)
                {
                    comps[selectedTab].Tab.FillTab(inRect, pawn);
                }
            }
        }
    }
}