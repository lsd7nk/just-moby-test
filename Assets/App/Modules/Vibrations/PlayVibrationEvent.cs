using Lofelt.NiceVibrations;

namespace App.Vibrations
{
    public struct PlayVibrationEvent
    {
        public HapticPatterns.PresetType VibrationType { get; private set; }

        public PlayVibrationEvent(HapticPatterns.PresetType vibrationType)
        {
            VibrationType = vibrationType;
        }
    }
}
