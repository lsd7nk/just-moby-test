using System;

namespace App.Popups
{
    public sealed class SettingsPopupView : PopupView
    {
        public override Type ServiceType => typeof(SettingsPopup);
    }
}
