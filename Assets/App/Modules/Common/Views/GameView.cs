using Doozy.Engine.UI;
using UnityEngine;
using App.Utils;
using System;

namespace App.Common.Views
{
    public partial class GameView : AdsView
    {
        [Header("Buttons")]
        [SerializeField] private UIButton _settingsButton;
    }


    public partial class GameView
    {
        public void AddSettingsButtonOnCickHandler(Action handler)
        {
            UIButtonUtils.AddOnCickHandler(_settingsButton, handler);
        }
    }
}