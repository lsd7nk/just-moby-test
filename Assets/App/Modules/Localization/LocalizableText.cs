using UnityEngine;
using TMPro;

namespace App.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class LocalizableText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private string _key;

        private void OnLocalize()
        {
            _label.text = LocalizationProvider.GetText(_key);
        }

        private void OnEnable()
        {
            OnLocalize();
            LocalizationProvider.OnLocalizeEvent += OnLocalize;
        }

        private void OnDisable()
        {
            LocalizationProvider.OnLocalizeEvent -= OnLocalize;
        }
    }
}