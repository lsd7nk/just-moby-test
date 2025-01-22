using UnityEngine.Events;
using Doozy.Engine.UI;
using UnityEngine;
using App.Utils;
using App.UI;
using System;
using TMPro;

namespace App.Popups
{
    public partial class SettingsPopupView : PopupView
    {
        public override Type ServiceType => typeof(SettingsPopup);

        [Header("Buttons")]
        [SerializeField] private UIButton _languageButton;
        [SerializeField] private UIButton _continueButton;
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _lobbyButton;

        [Header("Labels")]
        [SerializeField] private TMP_Text _languageLabel;
        [SerializeField] private TMP_Text _versionLabel;

        [Header("Toggles")]
        [SerializeField] private Toggle _vibrationToggle;

        public void RefreshButtons(bool inGameState)
        {
            _continueButton.gameObject.SetActive(inGameState);
            _restartButton.gameObject.SetActive(inGameState);
            _lobbyButton.gameObject.SetActive(inGameState);
        }

        public void SetVibrationToggleState(bool state)
        {
            _vibrationToggle.IsOn = state;
        }

        public void SetLanguageText(string value)
        {
            _languageLabel.text = value;
        }

        public void SetVersionText(string value)
        {
            _versionLabel.text = value;
        }
    }


    public partial class SettingsPopupView
    {
        public void AddVibrationToggleOnValueChanged(UnityAction<bool> handler)
        {
            _vibrationToggle.OnValueChangeEvent += handler;
        }

        public void AddLanguageButtonOnCickHandler(Action handler)
        {
            UIButtonUtils.AddOnCickHandler(_languageButton, handler);
        }

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
