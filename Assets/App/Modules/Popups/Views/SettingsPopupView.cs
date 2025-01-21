using Doozy.Engine.UI;
using UnityEngine;
using App.Utils;
using System;

namespace App.Popups
{
    public partial class SettingsPopupView : PopupView
    {
        public override Type ServiceType => typeof(SettingsPopup);

        [Header("Buttons")]
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _lobbyButton;

        public void RefreshButtons(bool inGameState)
        {
            _restartButton.gameObject.SetActive(inGameState);
            _lobbyButton.gameObject.SetActive(inGameState);
        }
    }


    public partial class SettingsPopupView
    {
        public void AddRestartButtonOnCickHandler(Action handler)
        {
            UIButtonUtils.AddOnCickHandler(_restartButton, handler);
        }

        public void AddLobbyButtonOnCickHandler(Action handler)
        {
            UIButtonUtils.AddOnCickHandler(_lobbyButton, handler);
        }
    }
}
