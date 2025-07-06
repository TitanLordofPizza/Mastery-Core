using System;

using UnityEngine;
using Verse;

namespace Mastery.Core.Utility.UI
{
    public class FieldWindow : Window
    {
        protected override void SetInitialSizeAndPosition()
        {
            Vector2 position = Verse.UI.MousePositionOnUIInverted - (InitialSize / 2);

            if (position.x + InitialSize.x > Verse.UI.screenWidth)
            {
                position.x = Verse.UI.screenWidth - InitialSize.x;
            }

            if (position.y + InitialSize.y > Verse.UI.screenHeight)
            {
                position.y = Verse.UI.screenHeight - InitialSize.y;
            }

            windowRect = new Rect(position.x, position.y, InitialSize.x, InitialSize.y);
        }

        public override void DoWindowContents(Rect inRect)
        {
        }
    }

    public class Vector2Window : FieldWindow
    {
        public string Title;

        public Vector2 Value;

        public Action<Vector2, Vector2> OnValueChanged;

        private Vector2 initialSize = new Vector2(400, 150);
        public override Vector2 InitialSize => initialSize;

        public Vector2Window(string title, Vector2 value)
        {
            Title = title;
            Value = value;

            var size = Text.CalcSize(title).x * 4;

            if(size > initialSize.x)
                initialSize.x = size;
        }

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            var standard = new Listing_Standard();
            standard.Begin(inRect);

            standard.Label(Title);

            Vector2 originalValue = new Vector2(Value.x, Value.y);

            string xBuffer = Value.x.ToString();
            standard.TextFieldNumericLabeled("x", ref Value.x, ref xBuffer, -1E+09f);

            string yBuffer = Value.y.ToString();
            standard.TextFieldNumericLabeled("y", ref Value.y, ref yBuffer, -1E+09f);

            standard.End();

            if (originalValue != Value)
            {
                OnValueChanged?.Invoke(originalValue, Value);
            }

            if (!inRect.Contains(Event.current.mousePosition))
            {
                float num = GenUI.DistFromRect(inRect, Event.current.mousePosition);

                if (num > 20)
                {
                    Close(true);
                }
            }
        }
    }
}