using Doozy.Engine.Progress;
using UnityEngine.Events;
using Doozy.Engine.UI;
using UnityEngine;

namespace App.UI
{
    [RequireComponent(typeof(UIToggle))]
    public sealed class Toggle : MonoBehaviour
    {
        public event UnityAction<bool> OnValueChangeEvent;

        public bool Interactable
        {
            get => _toggle.Interactable;
            set => _toggle.Interactable = value;
        }

        public bool IsOn
        {
            get => _toggle.IsOn;
            set
            {
                _toggle.IsOn = value;
                _progressor?.SetValue(value ? 1 : 0, true);
            }
        }

        [SerializeField] private UIToggle _toggle;
        [SerializeField] private Progressor _progressor;

        private void Start()
        {
            _toggle.OnValueChanged.AddListener(OnValueChangeEvent);
        }

        private void OnDestroy()
        {
            _toggle.OnValueChanged.RemoveListener(OnValueChangeEvent);
        }
    }
}
