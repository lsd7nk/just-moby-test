using Lofelt.NiceVibrations;
using Doozy.Engine.UI;
using App.Attributes;
using UnityEngine;
using App.Events;

namespace App.Vibrations
{
    [RequireComponent(typeof(UIButton))]
    public sealed class VibrationButton : MonoBehaviour
    {
#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField] private HapticPatterns.PresetType _type;

        public void SendPlayEvent()
        {
            EventSystem.Send(new PlayVibrationEvent(_type));
        }
    }
}
