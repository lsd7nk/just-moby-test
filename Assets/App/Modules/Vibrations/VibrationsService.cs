using Lofelt.NiceVibrations;
using App.Events;
using App.Data;
using System;

namespace App.Vibrations
{
    public sealed class VibrationsService : IDisposable
    {
        public bool IsEnabled
        {
            get => HapticController.hapticsEnabled;
            set
            {
                HapticController.hapticsEnabled = value;

                if (value)
                {
                    Initialize();
                }
                else
                {
                    RemoveListeners();
                }
            }
        }
        public bool Initialized { get; private set; }

        public VibrationsService(UserDataService userData)
        {
            IsEnabled = userData.VibrationEnabled;

            if (!IsEnabled)
            {
                return;
            }

            Initialize();
        }

        public void Dispose()
        {
            RemoveListeners();
        }

        public void PlaySelectionVibration()
        {
            PlayVibration(HapticPatterns.PresetType.Selection);
        }

        public void PlaySuccessVibration()
        {
            PlayVibration(HapticPatterns.PresetType.Success);
        }

        public void PlayFailureVibration()
        {
            PlayVibration(HapticPatterns.PresetType.Failure);
        }

        public void PlayLightImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.LightImpact);
        }

        public void PlayMediumImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.MediumImpact);
        }

        public void PlayHeavyImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.LightImpact);
        }

        public void PlayRigidImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.RigidImpact);
        }

        public void PlaySoftImpactVibration()
        {
            PlayVibration(HapticPatterns.PresetType.SoftImpact);
        }

        private void PlayVibration(PlayVibrationEvent e)
        {
            PlayVibration(e.VibrationType);
        }

        private void PlayVibration(HapticPatterns.PresetType vibrationType)
        {
            if (!IsEnabled)
            {
                return;
            }

            HapticPatterns.PlayPreset(vibrationType);
        }

        private void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            HapticController.Init();
            EventSystem.Subscribe<PlayVibrationEvent>(PlayVibration);

            Initialized = true;
        }

        private void RemoveListeners()
        {
            EventSystem.Unsubscribe<PlayVibrationEvent>(PlayVibration);

            Initialized = false;
        }
    }
}
