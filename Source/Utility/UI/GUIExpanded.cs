using UnityEngine;
using Verse;

namespace Mastery.Core.Utility.UI
{
    public static class GUIExpanded
    {
        public const float tinyUISpacing = 7.5f;
        public const float smallUISpacing = 15f;
        public const float mediumUISpacing = 22.5f;
        public const float largeUISpacing = 30f;

        public static void Foldout(Listing_Standard standard, string title, ref bool isExpanded)
        {
            string foldoutState = isExpanded ? "v" : ">";

            var foldoutSize = Text.CalcSize(foldoutState);
            var rect = standard.GetRect(foldoutSize.y);

            var foldoutRect = new Rect();
            var titleRect = new Rect();

            rect.SplitVerticallyWithMargin(out foldoutRect, out titleRect, out var _, 6, foldoutSize.x);

            if (Widgets.ButtonText(foldoutRect, foldoutState, false) == true)
            {
                isExpanded = !isExpanded;
            }

            Widgets.LabelFit(titleRect, title);
        }
    }
}