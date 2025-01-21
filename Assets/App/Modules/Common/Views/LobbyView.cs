using Doozy.Engine.UI;
using UnityEngine;
using App.Utils;
using System;

namespace App.Common.Views
{
    public partial class LobbyView : AdsView
    {
        [Header("Buttons")]
        [SerializeField] private UIButton _settingsButton;
        [SerializeField] private UIButton _playButton;
    }


    public partial class LobbyView
    {
        public void AddSettingsButtonOnCickHandler(Action handler)
        {
            UIButtonUtils.AddOnCickHandler(_settingsButton, handler);
        }

        public void AddPlayButtonOnCickHandler(Action handler)
        {
            UIButtonUtils.AddOnCickHandler(_playButton, handler);
        }
    }
}