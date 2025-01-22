using App.Localization;
using UnityEngine;
using System;
using App.UI;

namespace App.Popups
{
    public partial class LanguagesPopupView : PopupView
    {
        public override Type ServiceType => typeof(LanguagesPopup);

        [Header("Toggles")]
        [SerializeField] private LanguageToggle[] _languageToggles;

        public void SetCurrentLanguage(LanguageType currentLanguage)
        {
            for (int i = 0; i < _languageToggles.Length; ++i)
            {
                var toggle = _languageToggles[i];

                toggle.SetState(toggle.Language == currentLanguage);
            }
        }
    }


    public partial class LanguagesPopupView
    {
        public void AddTogglesOnClickHandler(Action<LanguageType> handler)
        {
            for (int i = 0; i < _languageToggles.Length; ++i)
            {
                _languageToggles[i].OnClickEvent += handler;
            }
        }
    }
}