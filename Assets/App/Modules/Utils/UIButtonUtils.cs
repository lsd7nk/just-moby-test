using Doozy.Engine.UI;
using System;

namespace App.Utils
{
    public static class UIButtonUtils
    {
        public static void AddOnPointerDownHandler(UIButton button, Action handler)
        {
            if (button == null)
            {
                return;
            }

            button.AllowMultipleClicks = false;
            button.OnClick.Enabled = true; // needed for disabling AllowMultipleClicks
            button.OnPointerDown.Enabled = true;
            button.OnPointerDown.OnTrigger.Action += (obj) => handler?.Invoke();
        }

        public static void AddOnCickHandler(UIButton button, Action handler)
        {
            if (button == null)
            {
                return;
            }

            button.AllowMultipleClicks = false;
            button.OnClick.Enabled = true; // needed for disabling AllowMultipleClicks
            button.OnClick.OnTrigger.Action += (obj) => handler?.Invoke();
        }
    }
}
