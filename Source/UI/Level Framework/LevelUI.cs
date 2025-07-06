using UnityEngine;
using RimWorld;
using Verse;

using Mastery.Core.Settings.Level_Framework;
using Mastery.Core.Data.Level_Framework.Comps;

namespace Mastery.Core.UI.Level_Framework
{
    [StaticConstructorOnStartup]
    public static class LevelUI
    {
        private static Texture2D SkillBarBackgroundTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));
        private static Texture2D SkillBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

        public static void Field(Listing_Standard standard, string defName, Level_Comp comp, float labelCapWidth)
        {
            var title = Level_Settings_Manager.Instances[comp.LevelKey].GetLabelCap(defName);

            var inRect = standard.GetRect(Text.CalcHeight(title, labelCapWidth));

            var labelRect = new Rect(inRect.x, inRect.y, labelCapWidth, inRect.height);

            Widgets.Label(labelRect, title);

            var passionRect = new Rect(labelRect.xMax, inRect.y, 24f, 24f);

            if (comp.Entries[defName].Passion > 0)
            {
                Texture2D image = ((comp.Entries[defName].Passion == Passion.Major) ? SkillUI.PassionMajorIcon : SkillUI.PassionMinorIcon);
                GUI.DrawTexture(passionRect, image);
            }

            var config = Level_Settings_Manager.Instances[comp.LevelKey].IGetConfig(defName);

            Rect barRect = new Rect(passionRect.xMax, inRect.y, inRect.width - passionRect.xMax, 24f);

            Widgets.FillableBar(barRect, Mathf.Max(0.01f, (float)comp.Entries[defName].Level / config.ExpCurve.MaxX), SkillBarFillTex, SkillBarBackgroundTex, doBorder: true);

            Rect levelRect = new Rect(passionRect.xMax + 4f, inRect.y, 999f, inRect.height);
            levelRect.yMin += 3f;

            Widgets.Label(levelRect, comp.Entries[defName].Level.ToString());

            if (Mouse.IsOver(inRect))
            {
                TooltipHandler.TipRegion(inRect, comp.GetDescription(defName, true));
            }
        }
    }
}