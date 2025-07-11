using System.Collections.Generic;

using UnityEngine;
using Verse;

using Mastery.Core.Utility.UI;

namespace Mastery.Core.Utility
{
    public class UtilityCurve : IExposable, IDuplicable<UtilityCurve>
    {
        public SimpleCurve Curve;
        public bool Percentage; //Only Used for the Value not the Point.

        public float MinX => Curve.Points[0].x;

        public float MaxX => Curve.Points[Curve.PointsCount - 1].x;

        public float MinY => Curve.MinY;
        public float MaxY => Curve.MaxY;

        public void ExposeData()
        {
            Scribe_Deep.Look(ref Curve, "curve");

            Scribe_Values.Look(ref Percentage, "percentage");
        }

        public float Evaluate(float x, int decimalPoint = 2)
        {
            return Math.RoundUp(Curve.Evaluate(x), decimalPoint);
        }

        public void CopyTo(UtilityCurve target)
        {
            var curve = new SimpleCurve();

            if (Curve?.Points != null)
            {
                foreach (var point in Curve.Points)
                {
                    curve.Add(new CurvePoint(point.x, point.y));
                }
            }

            target.Curve = curve;

            target.Percentage = Percentage;
        }

        public UtilityCurve Duplicate()
        {
            var duplicate = new UtilityCurve();

            CopyTo(duplicate);

            return duplicate;
        }

        #region UI

        public const float UIHeight = 200;

        public void Editor(Listing_Standard standard, string title, int decimalPoints = 2, bool showPercentage = true, bool active = true)
        {
            standard.Label(title);

            var screenRect = standard.GetRect(UIHeight);

            Widgets.DrawMenuSection(screenRect);
            SimpleCurveDrawer.DrawCurve(screenRect, Curve);

            Vector2 mousePosition = Event.current.mousePosition - screenRect.position;
            Vector2 mouseCurveCoords = SimpleCurveDrawer.ScreenToCurveCoords(screenRect, Curve.View.rect, mousePosition);

            if (active == true && Mouse.IsOver(screenRect) == true)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    var valueSuffix = Percentage ? "%" : "";

                    Vector2 inclosedRange = new Vector2(Curve.Points[0].x / Curve.Points[Curve.PointsCount - 1].x, Curve.Points[0].y / Curve.Points[Curve.PointsCount - 1].y);
                    var roundedCords = new Vector2(Math.RoundUp(mouseCurveCoords.x, decimalPoints), Math.RoundUp(mouseCurveCoords.y, decimalPoints));

                    List<FloatMenuOption> options = new List<FloatMenuOption>
                    {
                        new FloatMenuOption($"Add point at [{roundedCords.x:F0} - {(roundedCords.y).ToString($"F{decimalPoints}")}{valueSuffix}]", () =>
                        {
                            Curve.Add(new CurvePoint(roundedCords), true);
                            Curve.View.SetViewRectAround(Curve);
                        }, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
                    };

                    int closestPoint = 0;
                    int index = 0;

                    Vector2 mousePercent = new Vector2(roundedCords.x / MaxX, roundedCords.y / MaxY);

                    foreach (var curvePoint in Curve.Points)
                    {
                        if (Vector2.Distance(new Vector2(curvePoint.x / MaxX, curvePoint.y / MaxY), mousePercent) <
                            Vector2.Distance(new Vector2(Curve.Points[closestPoint].x / MaxX, Curve.Points[closestPoint].y / MaxY), mousePercent))
                        {
                            closestPoint = index;
                        }

                        index++;
                    }

                    var existingPoint = Curve.Points[closestPoint];

                    options.Add(new FloatMenuOption($"Move point at [{existingPoint.x:F0} - {(existingPoint.y).ToString($"F{decimalPoints}")}{valueSuffix}] to [{roundedCords.x:F0} - {(roundedCords.y).ToString($"F{decimalPoints}")}{valueSuffix}]", () =>
                    {
                        Curve.RemovePointNear(existingPoint);
                        Curve.Add(new CurvePoint(roundedCords), true);
                        Curve.View.SetViewRectAround(Curve);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));

                    options.Add(new FloatMenuOption($"Manually Move Point at [{existingPoint.x:F0} - {(existingPoint.y).ToString($"F{decimalPoints}")}{valueSuffix}]", () =>
                    {
                        var manualPosition = new Vector2(existingPoint.x, existingPoint.y);

                        var vector2Window = new Vector2Window($"{title} [{existingPoint.x:F0} - {(existingPoint.y).ToString($"F{decimalPoints}")}{valueSuffix}]", manualPosition);

                        vector2Window.OnValueChanged += OnValueChanged;

                        Find.WindowStack.Add(vector2Window);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));

                    if (closestPoint != 0 && closestPoint != Curve.PointsCount - 1)
                    {
                        options.Add(new FloatMenuOption($"Remove point at [{existingPoint.x:F0} - {(existingPoint.y).ToString($"F{decimalPoints}")}{valueSuffix}]", () =>
                        {
                            Curve.RemovePointNear(existingPoint);
                            Curve.View.SetViewRectAround(Curve);
                        }, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
                    }

                    Find.WindowStack.Add(new FloatMenu(options));
                    Event.current.Use();
                }
            }

            if (showPercentage == true && active == true)
            {
                standard.CheckboxLabeled("Percentage", ref Percentage);
            }
        }

        private void OnValueChanged(Vector2 originalPosition, Vector2 newPosition)
        {
            CurvePoint oldPoint = Curve.Points[0];

            foreach (var point in Curve.Points)
            {
                if (Vector2.Distance(new Vector2(point.x, point.y), originalPosition) < Vector2.Distance(oldPoint, originalPosition))
                {
                    oldPoint = point;
                }
            }

            Curve.RemovePointNear(oldPoint);
            Curve.Add(new CurvePoint(newPosition), true);
            Curve.View.SetViewRectAround(Curve);
        }

        #endregion
    }
}