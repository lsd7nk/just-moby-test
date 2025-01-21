using App.Localization;
using Doozy.Engine.UI;
using UnityEngine;
using System;

namespace App.UI
{
    [RequireComponent(typeof(UIToggle))]
    public sealed class LanguageToggle : MonoBehaviour
    {
        public event Action<LanguageType> OnClickEvent;

        [field: SerializeField] public LanguageType Language { get; private set; }
        [SerializeField] private UIToggle _toggle;

        public void SetState(bool state)
        {
            _toggle.IsOn = state;
        }

        private void OnValueChanged(bool value)
        {
            if (!value)
            {
                return;
            }

            OnClickEvent?.Invoke(Language);
        }

        private void Initialize()
        {
            _toggle.OnValueChanged.AddListener(OnValueChanged);
        }

        private void Awake()
        {
            Initialize();
        }
    }
}