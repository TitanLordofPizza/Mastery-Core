using UnityEngine;
using Verse;

namespace Mastery.Core.UI.Tabs
{
    public interface IMastery_Tab
    {
        string Title { get; }

        void FillTab(Rect inRect, Pawn pawn);
    }
}