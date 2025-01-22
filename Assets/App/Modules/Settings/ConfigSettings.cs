using UnityEngine;

namespace App.Settings
{
    [CreateAssetMenu(menuName = "Settings/Config", fileName = "ConfigSettings", order = 52)]
    public sealed class ConfigSettings : ScriptableObject
    {
        [field: SerializeField] public bool VibrationEnabled { get; private set; } = true;
        [field: SerializeField] public bool BannerAdEnabled { get; private set; } = true;
        [field: SerializeField] public int FiguresCount { get; private set; } = 20;
    }
}