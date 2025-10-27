using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;
using RimWorld;
using Verse;

using Mastery.Core.Data.Level_Framework.Comps;
using Mastery.Core.Settings.Level_Framework;

namespace Mastery.Core.Utility.UI
{
    [StaticConstructorOnStartup]
    public static class UIUtility
    {
        public const float tinyUISpacing = 8;
        public const float smallUISpacing = 16;
        public const float mediumUISpacing = 24;
        public const float largeUISpacing = 32;

        public static void Foldout(Listing_Standard standard, string title, ref bool is_collapsed)
        {
            string foldout_state = is_collapsed ? ">" : "v";

            var foldout_size = Text.CalcSize(foldout_state);
            var rect = standard.GetRect(foldout_size.y);

            rect.SplitVerticallyWithMargin(out Rect foldout_rect, out Rect title_rect, out _, tinyUISpacing, foldout_size.x);

            if (Widgets.ButtonText(foldout_rect, foldout_state, false) == true)
            {
                is_collapsed = !is_collapsed;
            }

            Widgets.LabelFit(title_rect, title);
        }

        public static void Foldout_Checkbox(Listing_Standard standard, string title, string checkbox_title, ref bool is_collapsed, ref bool checkbox_value)
        {
            string foldout_state = is_collapsed ? ">" : "v";

            var foldout_size = Text.CalcSize(foldout_state);
            var rect = standard.GetRect(foldout_size.y);

            rect.SplitVerticallyWithMargin(out Rect foldout_rect, out Rect title_rect, out _, tinyUISpacing, foldout_size.x);

            title_rect.SplitVerticallyWithMargin(out title_rect, out Rect checkbox_rect, out _, tinyUISpacing, Text.CalcSize(title).x);

            if (Widgets.ButtonText(foldout_rect, foldout_state, false) == true)
            {
                is_collapsed = !is_collapsed;
            }

            Widgets.LabelFit(title_rect, title);

            Widgets.CheckboxLabeled(checkbox_rect, checkbox_title, ref checkbox_value);
        }

        public static void Dropdown(Listing_Standard standard, string title, int selectedOption, List<string> options, Action<int> onSelected)
        {
            title = $"{title}:";

            var titleRect = new Rect();
            var dropdownRect = new Rect();

            standard.GetRect(mediumUISpacing).SplitVerticallyWithMargin(out titleRect, out dropdownRect, out _, tinyUISpacing, Text.CalcSize(title).x);

            dropdownRect.SplitVerticallyWithMargin(out dropdownRect, out _, out _, 0, Text.CalcSize(options.OrderByDescending(option => option.Length).FirstOrDefault()).x);

            string selected = options[selectedOption];

            Widgets.LabelFit(titleRect, title);

            Widgets.Dropdown(dropdownRect,
                selected,
                o => o,
                o => options.Select(optionValue => new Widgets.DropdownMenuElement<string>
                {
                    option = new FloatMenuOption(optionValue, () =>
                    {
                        onSelected(options.IndexOf(optionValue));
                    }),
                    payload = optionValue
                }), options[selectedOption]);
        }

        public static void Tabs(Rect rect, List<TabRecord> tabs, bool menuSection = true)
        {
            rect.y += largeUISpacing;

            if (menuSection == true)
            {
                Widgets.DrawMenuSection(rect);
            }

            TabDrawer.DrawTabs(rect, tabs);
        }

        private static Texture2D SkillBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

        public static void LevelInfo(Listing_Standard standard, string defName, Level_Comp comp, float labelCapWidth)
        {
            var title = Level_Settings_Manager.Instances[comp.LevelKey].GetLabelCap(defName);

            var inRect = standard.GetRect(Text.CalcHeight(title, labelCapWidth));
            var splitRect = inRect;

            splitRect.SplitVerticallyWithMargin(out Rect labelRect, out splitRect, out _, tinyUISpacing, labelCapWidth);

            Widgets.Label(labelRect, title);

            splitRect.SplitVerticallyWithMargin(out Rect passionRect, out splitRect, out _, tinyUISpacing / 4, mediumUISpacing);
            passionRect.height = mediumUISpacing;

            if (comp.Entries[defName].passion > 0)
            {
                GUI.DrawTexture(passionRect, (comp.Entries[defName].passion == Passion.Major) ? SkillUI.PassionMajorIcon : SkillUI.PassionMinorIcon);
            }

            var config = Level_Settings_Manager.Instances[comp.LevelKey].IGetConfig(defName);

            splitRect.SplitVerticallyWithMargin(out Rect barRect, out splitRect, out _, 0, rightWidth: mediumUISpacing);
            barRect.height = mediumUISpacing;

            Widgets.FillableBar(barRect, comp.Entries[defName].exp / config.ExpCalculated(comp.Entries[defName].level), SkillBarFillTex, null, doBorder: false);

            Rect levelRect = new Rect(barRect.xMin + tinyUISpacing, inRect.y, 999f, inRect.height);
            levelRect.yMin += 3f;

            Widgets.Label(levelRect, comp.Entries[defName].level.ToString());

            if (Mouse.IsOver(inRect))
            {
                TooltipHandler.TipRegion(inRect, comp.GetDescription(defName, true));
            }
        }
    }
}