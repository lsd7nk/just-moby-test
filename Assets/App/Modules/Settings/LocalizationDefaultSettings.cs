using App.Localization;
using UnityEngine;

namespace App.Settings
{
    [CreateAssetMenu(fileName = "LocalizationDefaultSettings", menuName = "Settings/Localization default", order = 0)]
    public sealed class LocalizationDefaultSettings : ScriptableObject
    {
        [field: SerializeField] public LocalizedText DefaultLocalizationFile { get; private set; }
    }
}